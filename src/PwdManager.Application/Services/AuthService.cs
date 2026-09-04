using System.Security.Cryptography;
using PwdManager.Application.Configuration;
using PwdManager.Application.Interfaces;
using PwdManager.Domain.Security;

namespace PwdManager.Application.Services;

/// <summary>
/// Login, reveal-time re-authentication and password change. On successful login the
/// user's password unwraps their copy of the DEK into a <see cref="SessionContext"/>.
/// The UI only ever sees <see cref="LoginStatus"/> — never AES/Argon2/EF/MySQL.
/// </summary>
public sealed class AuthService
{
    private readonly IUserRepository _users;
    private readonly IAuditRepository _audit;
    private readonly SecurityConfig _security;
    private readonly IPasswordHasher _hasher;
    private readonly IKeyDerivation _kdf;
    private readonly IDataProtector _crypto;

    // Verified against for unknown usernames so login timing does not leak whether an
    // account exists (mitigates username enumeration).
    private readonly string _dummyHash;

    public AuthService(IUserRepository users, IAuditRepository audit, SecurityConfig security,
        IPasswordHasher hasher, IKeyDerivation kdf, IDataProtector crypto)
    {
        _users = users;
        _audit = audit;
        _security = security;
        _hasher = hasher;
        _kdf = kdf;
        _crypto = crypto;
        _dummyHash = hasher.Hash("timing-equalizer-not-a-real-password");
    }

    public enum LoginStatus { Success, InvalidCredentials, Inactive, LockedOut }

    public sealed record LoginOutcome(LoginStatus Status, SessionContext? Session, DateTime? LockedUntil);

    public async Task<LoginOutcome> LoginAsync(string username, string password, CancellationToken ct = default)
    {
        username = username.Trim();
        var user = await _users.FindByUsernameAsync(username, ct);
        if (user is null)
        {
            _hasher.Verify(password, _dummyHash); // equalise timing
            await _audit.WriteAsync(AuditAction.LoginFailed, null, username, detail: "unknown user", ct: ct);
            return new LoginOutcome(LoginStatus.InvalidCredentials, null, null);
        }

        if (user.LockedUntil is { } lockedUntil && lockedUntil > DateTime.UtcNow)
            return new LoginOutcome(LoginStatus.LockedOut, null, lockedUntil);

        if (!user.IsActive)
            return new LoginOutcome(LoginStatus.Inactive, null, null);

        if (!_hasher.Verify(password, user.PasswordHash))
        {
            int failed = user.FailedLoginCount + 1;
            DateTime? newLock = failed >= _security.LoginMaxAttempts
                ? DateTime.UtcNow.AddMinutes(_security.LockoutMinutes)
                : null;
            await _users.RegisterFailedLoginAsync(user.Id, failed, newLock, ct);
            await _audit.WriteAsync(AuditAction.LoginFailed, user.Id, user.Username, detail: $"attempt {failed}", ct: ct);
            return new LoginOutcome(LoginStatus.InvalidCredentials, null, newLock);
        }

        byte[] kek = _kdf.DeriveKey(password, user.KdfSalt);
        byte[] dek;
        try
        {
            dek = _crypto.UnwrapKey(kek, user.WrappedDek);
        }
        catch (CryptographicException)
        {
            await _audit.WriteAsync(AuditAction.LoginFailed, user.Id, user.Username, detail: "DEK unwrap failed", ct: ct);
            return new LoginOutcome(LoginStatus.InvalidCredentials, null, null);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(kek);
        }

        await _users.ClearLoginFailuresAsync(user.Id, ct);
        await _audit.WriteAsync(AuditAction.Login, user.Id, user.Username, ct: ct);

        var identity = new AuthenticatedUser(user.Id, user.Username, user.FullName, user.Role);

        var session = new SessionContext(identity, dek, _crypto);
        CryptographicOperations.ZeroMemory(dek);

        return new LoginOutcome(LoginStatus.Success, session, null);
    }

    /// <summary>
    /// Reveal-time re-authentication: the password must both verify AND actually unwrap
    /// the DEK. Failures are audited (shoulder-surf / hijacked-session signal).
    /// </summary>
    public async Task<bool> VerifyPasswordAsync(SessionContext session, string password, CancellationToken ct = default)
    {
        var user = await _users.GetByIdAsync(session.User.Id, ct);
        bool ok = user is not null && _hasher.Verify(password, user.PasswordHash);

        if (ok)
        {
            byte[] kek = _kdf.DeriveKey(password, user!.KdfSalt);
            try
            {
                byte[] dek = _crypto.UnwrapKey(kek, user.WrappedDek);
                CryptographicOperations.ZeroMemory(dek);
            }
            catch (CryptographicException)
            {
                ok = false;
            }
            finally
            {
                CryptographicOperations.ZeroMemory(kek);
            }
        }

        if (!ok)
            await _audit.WriteAsync(AuditAction.RevealAuthFailed, session.User.Id, session.User.Username,
                detail: "reveal re-auth failed", ct: ct);

        return ok;
    }

    public async Task<bool> MustChangePasswordAsync(long userId, CancellationToken ct = default)
    {
        var user = await _users.GetByIdAsync(userId, ct);
        return user?.MustChangePassword == true;
    }

    /// <summary>Live check for an existing session: has the account been deactivated/deleted?</summary>
    public Task<bool> IsAccountActiveAsync(long userId, CancellationToken ct = default)
        => _users.IsActiveAsync(userId, ct);

    /// <summary>Re-derives the DEK from the current password and re-wraps it under the new one.</summary>
    public async Task ChangePasswordAsync(SessionContext session, string currentPassword, string newPassword,
        CancellationToken ct = default)
    {
        PasswordPolicy.Ensure(newPassword);

        var user = await _users.GetByIdAsync(session.User.Id, ct)
                   ?? throw new InvalidOperationException("Kullanıcı bulunamadı.");

        if (!_hasher.Verify(currentPassword, user.PasswordHash))
            throw new InvalidOperationException("Mevcut parola hatalı.");

        if (newPassword == currentPassword)
            throw new InvalidOperationException("Yeni parola eskisiyle aynı olamaz.");

        byte[] oldKek = _kdf.DeriveKey(currentPassword, user.KdfSalt);
        byte[] dek;
        try
        {
            dek = _crypto.UnwrapKey(oldKek, user.WrappedDek);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(oldKek);
        }

        try
        {
            byte[] newSalt = _kdf.NewSalt();
            byte[] newKek = _kdf.DeriveKey(newPassword, newSalt);
            byte[] newWrapped = _crypto.WrapKey(newKek, dek);
            CryptographicOperations.ZeroMemory(newKek);

            await _users.SetCredentialsAsync(
                user.Id, _hasher.Hash(newPassword), newSalt, newWrapped,
                mustChangePassword: false, ct);
            await _audit.WriteAsync(AuditAction.PasswordChange, user.Id, user.Username, ct: ct);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(dek);
        }
    }
}
