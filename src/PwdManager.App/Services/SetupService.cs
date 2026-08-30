using System.Security.Cryptography;
using System.Text;
using PwdManager.Core.Cryptography;
using PwdManager.Data.Entities;
using PwdManager.Data.Repositories;

namespace PwdManager.App.Services;

/// <summary>
/// One-time initialisation: generates the system DEK, creates the first admin with a
/// password-wrapped copy of it, and stores a recovery-code-wrapped copy for disaster recovery.
/// </summary>
public sealed class SetupService
{
    private readonly IUserRepository _users;
    private readonly IAppMetaRepository _meta;

    public SetupService(IUserRepository users, IAppMetaRepository meta)
    {
        _users = users;
        _meta = meta;
    }

    public sealed record Result(string RecoveryCode);

    public async Task<Result> CreateFirstAdminAsync(
        string username, string password, string fullName, CancellationToken ct = default)
    {
        byte[] dek = DataKey.Generate();
        try
        {
            // --- Admin's password-wrapped copy of the DEK ---
            byte[] adminSalt = KeyDerivation.NewSalt();
            byte[] adminKek = KeyDerivation.DeriveKek(password, adminSalt);
            byte[] adminWrapped = DataKey.Wrap(adminKek, dek);
            CryptographicOperations.ZeroMemory(adminKek);

            await _users.CreateAsync(new User
            {
                Username = username.Trim(),
                FullName = fullName.Trim(),
                Role = "Admin",
                PasswordHash = PasswordHasher.Hash(password),
                KdfSalt = adminSalt,
                WrappedDek = adminWrapped,
                IsActive = true,
                MustChangePw = false
            }, ct);

            // --- Recovery-code-wrapped copy of the DEK ---
            string recoveryCode = RecoveryCode.Generate();
            byte[] recoverySalt = KeyDerivation.NewSalt();
            byte[] recoveryKek = KeyDerivation.DeriveKek(RecoveryCode.Normalize(recoveryCode), recoverySalt);
            byte[] recoveryWrapped = DataKey.Wrap(recoveryKek, dek);
            CryptographicOperations.ZeroMemory(recoveryKek);

            await _meta.SetAsync(AppMetaKeys.RecoverySalt, recoverySalt, ct);
            await _meta.SetAsync(AppMetaKeys.RecoveryWrappedDek, recoveryWrapped, ct);
            await _meta.SetAsync(AppMetaKeys.SchemaVersion, Encoding.UTF8.GetBytes("1"), ct);

            return new Result(recoveryCode);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(dek);
        }
    }
}
