using System.Security.Cryptography;
using PwdManager.Application.Interfaces;
using PwdManager.Application.Models;
using PwdManager.Domain.Enums;
using PwdManager.Domain.Security;

namespace PwdManager.Application.Services;

/// <summary>
/// Admin-only personnel provisioning. Creating or resetting an account re-wraps the
/// admin session's DEK under the new user's password, so the personnel can unwrap the
/// same system DEK at their first login. The admin never learns the DEK bytes.
/// </summary>
public sealed class PersonnelService
{
    private readonly IUserRepository _users;
    private readonly IAuditRepository _audit;
    private readonly IPasswordHasher _hasher;
    private readonly IKeyDerivation _kdf;

    public PersonnelService(IUserRepository users, IAuditRepository audit,
        IPasswordHasher hasher, IKeyDerivation kdf)
    {
        _users = users;
        _audit = audit;
        _hasher = hasher;
        _kdf = kdf;
    }

    public async Task<long> CreateAsync(SessionContext admin, string username, string fullName,
        string initialPassword, CancellationToken ct = default)
    {
        admin.EnsureAdmin();

        byte[] salt = _kdf.NewSalt();
        byte[] kek = _kdf.DeriveKey(initialPassword, salt);
        byte[] wrapped;
        try
        {
            wrapped = admin.Protector.RewrapDekUnder(kek);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(kek);
        }

        long id = await _users.CreateAsync(new NewUser(
            Username: username.Trim(),
            FullName: fullName.Trim(),
            Role: UserRole.Personnel,
            PasswordHash: _hasher.Hash(initialPassword),
            KdfSalt: salt,
            WrappedDek: wrapped,
            IsActive: true,
            MustChangePassword: true), ct);

        await _audit.WriteAsync(AuditAction.UserAdd, admin.User.Id, admin.User.Username, "user", id, username.Trim(), ct);
        return id;
    }

    public async Task ResetPasswordAsync(SessionContext admin, long userId, string newInitialPassword,
        CancellationToken ct = default)
    {
        admin.EnsureAdmin();

        byte[] salt = _kdf.NewSalt();
        byte[] kek = _kdf.DeriveKey(newInitialPassword, salt);
        byte[] wrapped;
        try
        {
            wrapped = admin.Protector.RewrapDekUnder(kek);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(kek);
        }

        await _users.SetCredentialsAsync(userId, _hasher.Hash(newInitialPassword), salt, wrapped,
            mustChangePassword: true, ct);
        await _audit.WriteAsync(AuditAction.UserReset, admin.User.Id, admin.User.Username, "user", userId, ct: ct);
    }

    public async Task UpdateAsync(SessionContext admin, long userId, string fullName, bool isActive,
        CancellationToken ct = default)
    {
        admin.EnsureAdmin();
        await _users.UpdateProfileAsync(userId, fullName.Trim(), isActive, ct);
        await _audit.WriteAsync(AuditAction.UserUpdate, admin.User.Id, admin.User.Username, "user", userId,
            detail: isActive ? "active" : "disabled", ct: ct);
    }

    public Task<IReadOnlyList<UserRecord>> ListPersonnelAsync(CancellationToken ct = default) =>
        _users.ListPersonnelAsync(ct);
}
