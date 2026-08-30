using System.Security.Cryptography;
using PwdManager.App.Composition;
using PwdManager.Core.Cryptography;
using PwdManager.Core.Models;
using PwdManager.Core.Security;
using PwdManager.Data.Repositories;

namespace PwdManager.App.Services;

/// <summary>
/// Login, reveal-time re-authentication and password change. On successful login the
/// user's password unwraps their copy of the DEK into a <see cref="SessionContext"/>.
/// </summary>
public sealed class AuthService
{
    private readonly IUserRepository _users;
    private readonly IAuditRepository _audit;
    private readonly SecurityConfig _security;

    // Verified against for unknown usernames so login timing does not leak whether an
    // account exists (mitigates username enumeration).
    private static readonly string DummyHash = PasswordHasher.Hash("timing-equalizer-not-a-real-password");

    public AuthService(IUserRepository users, IAuditRepository audit, SecurityConfig security)
    {
        _users = users;
        _audit = audit;
        _security = security;
    }

    public enum LoginStatus { Success, InvalidCredentials, Inactive, LockedOut }

    public sealed record LoginOutcome(LoginStatus Status, SessionContext? Session, DateTime? LockedUntil);

    public async Task<LoginOutcome> LoginAsync(string username, string password, CancellationToken ct = default)
    {
        username = username.Trim();
        var user = await _users.FindByUsernameAsync(username, ct);
        if (user is null)
        {
            PasswordHasher.Verify(password, DummyHash); // equalise timing
            await _audit.WriteAsync(AuditAction.LoginFailed, null, username, detail: "unknown user", ct: ct);
            return new LoginOutcome(LoginStatus.InvalidCredentials, null, null);
        }

        if (user.LockedUntil is { } lockedUntil && lockedUntil > DateTime.UtcNow)
            return new LoginOutcome(LoginStatus.LockedOut, null, lockedUntil);

        if (user.IsActive != true)
            return new LoginOutcome(LoginStatus.Inactive, null, null);

        if (!PasswordHasher.Verify(password, user.PasswordHash))
        {
            int failed = user.FailedLoginCount + 1;
            DateTime? newLock = failed >= _security.LoginMaxAttempts ? DateTime.UtcNow.AddMinutes(15) : null;
            await _users.RegisterFailedLoginAsync(user.Id, failed, newLock, ct);
            await _audit.WriteAsync(AuditAction.LoginFailed, user.Id, user.Username, detail: $"attempt {failed}", ct: ct);
            return new LoginOutcome(LoginStatus.InvalidCredentials, null, newLock);
        }

        byte[] kek = KeyDerivation.DeriveKek(password, user.KdfSalt);
        byte[] dek;
        try
        {
            dek = DataKey.Unwrap(kek, user.WrappedDek);
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

        var identity = new AuthenticatedUser(
            user.Id, user.Username, user.FullName,
            user.Role == "Admin" ? UserRole.Admin : UserRole.Personnel);

        var session = new SessionContext(identity, dek);
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
        bool ok = user is not null && PasswordHasher.Verify(password, user.PasswordHash);

        if (ok)
        {
            byte[] kek = KeyDerivation.DeriveKek(password, user!.KdfSalt);
            try
            {
                byte[] dek = DataKey.Unwrap(kek, user.WrappedDek);
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
        return user?.MustChangePw == true;
    }

    /// <summary>Live check for an existing session: has the account been deactivated/deleted?</summary>
    public Task<bool> IsAccountActiveAsync(long userId, CancellationToken ct = default)
        => _users.IsActiveAsync(userId, ct);

    /// <summary>Re-derives the DEK from the current password and re-wraps it under the new one.</summary>
    public async Task ChangePasswordAsync(SessionContext session, string currentPassword, string newPassword,
        CancellationToken ct = default)
    {
        var user = await _users.GetByIdAsync(session.User.Id, ct)
                   ?? throw new InvalidOperationException("Kullanıcı bulunamadı.");

        if (!PasswordHasher.Verify(currentPassword, user.PasswordHash))
            throw new InvalidOperationException("Mevcut parola hatalı.");

        byte[] oldKek = KeyDerivation.DeriveKek(currentPassword, user.KdfSalt);
        byte[] dek;
        try
        {
            dek = DataKey.Unwrap(oldKek, user.WrappedDek);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(oldKek);
        }

        try
        {
            byte[] newSalt = KeyDerivation.NewSalt();
            byte[] newKek = KeyDerivation.DeriveKek(newPassword, newSalt);
            byte[] newWrapped = DataKey.Wrap(newKek, dek);
            CryptographicOperations.ZeroMemory(newKek);

            await _users.SetCredentialsAsync(
                user.Id, PasswordHasher.Hash(newPassword), newSalt, newWrapped,
                mustChangePassword: false, ct);
            await _audit.WriteAsync(AuditAction.PasswordChange, user.Id, user.Username, ct: ct);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(dek);
        }
    }
}
