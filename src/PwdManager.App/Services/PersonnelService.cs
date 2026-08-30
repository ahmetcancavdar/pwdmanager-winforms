using System.Security.Cryptography;
using PwdManager.Core.Cryptography;
using PwdManager.Core.Security;
using PwdManager.Data.Entities;
using PwdManager.Data.Repositories;

namespace PwdManager.App.Services;

/// <summary>
/// Admin-only personnel provisioning. Creating or resetting an account re-wraps the
/// admin session's DEK under the new user's password, so the personnel can unwrap the
/// same system DEK at their first login. The admin never learns the DEK bytes.
/// </summary>
public sealed class PersonnelService
{
    private readonly IUserRepository _users;
    private readonly IAuditRepository _audit;

    public PersonnelService(IUserRepository users, IAuditRepository audit)
    {
        _users = users;
        _audit = audit;
    }

    public async Task<long> CreateAsync(SessionContext admin, string username, string fullName,
        string initialPassword, CancellationToken ct = default)
    {
        admin.EnsureAdmin();

        byte[] salt = KeyDerivation.NewSalt();
        byte[] kek = KeyDerivation.DeriveKek(initialPassword, salt);
        byte[] wrapped;
        try
        {
            wrapped = admin.Protector.RewrapDekUnder(kek);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(kek);
        }

        long id = await _users.CreateAsync(new User
        {
            Username = username.Trim(),
            FullName = fullName.Trim(),
            Role = "Personnel",
            PasswordHash = PasswordHasher.Hash(initialPassword),
            KdfSalt = salt,
            WrappedDek = wrapped,
            IsActive = true,
            MustChangePw = true
        }, ct);

        await _audit.WriteAsync(AuditAction.UserAdd, admin.User.Id, admin.User.Username, "user", id, username.Trim(), ct);
        return id;
    }

    public async Task ResetPasswordAsync(SessionContext admin, long userId, string newInitialPassword,
        CancellationToken ct = default)
    {
        admin.EnsureAdmin();

        byte[] salt = KeyDerivation.NewSalt();
        byte[] kek = KeyDerivation.DeriveKek(newInitialPassword, salt);
        byte[] wrapped;
        try
        {
            wrapped = admin.Protector.RewrapDekUnder(kek);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(kek);
        }

        await _users.SetCredentialsAsync(userId, PasswordHasher.Hash(newInitialPassword), salt, wrapped,
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

    public Task<IReadOnlyList<User>> ListPersonnelAsync(CancellationToken ct = default) =>
        _users.ListPersonnelAsync(ct);
}
