using System.Security.Cryptography;
using System.Text;
using PwdManager.Application.Interfaces;
using PwdManager.Application.Models;
using PwdManager.Domain.Enums;
using PwdManager.Domain.Security;

namespace PwdManager.Application.Services;



/// <summary>
/// 1. Sistemin ana şifreleme anahtarını üretir: DEK
/// 2. İlk admin kullanıcısını oluşturur
///3.Admin parolasıyla DEK'i sarar: WrappedDek
///4. Kurtarma kodu üretir
///5. Aynı DEK'i recovery code ile de sarar
///6. Recovery bilgilerini app_meta tablosuna kaydeder
/// One-time initialisation: generates the system DEK, creates the first admin with a
/// password-wrapped copy of it, and stores a recovery-code-wrapped copy for disaster recovery.
/// </summary>
public sealed class SetupService
{
    private readonly IUserRepository _users;
    private readonly IAppMetaRepository _meta;
    private readonly IPasswordHasher _hasher;
    private readonly IKeyDerivation _kdf;
    private readonly IDataProtector _crypto;
    private readonly IRecoveryCodeService _recovery;

    public SetupService(IUserRepository users, IAppMetaRepository meta,
        IPasswordHasher hasher, IKeyDerivation kdf, IDataProtector crypto, IRecoveryCodeService recovery)
    {
        _users = users;
        _meta = meta;
        _hasher = hasher;
        _kdf = kdf;
        _crypto = crypto;
        _recovery = recovery;
    }

    public sealed record Result(string RecoveryCode);

    public async Task<Result> CreateFirstAdminAsync(
        string username, string password, string fullName, CancellationToken ct = default)
    {
        PasswordPolicy.Ensure(password);

        byte[] dek = _crypto.NewDataKey();
        try
        {
            // --- Admin's password-wrapped copy of the DEK ---
            byte[] adminSalt = _kdf.NewSalt();
            byte[] adminKek = _kdf.DeriveKey(password, adminSalt);
            byte[] adminWrapped = _crypto.WrapKey(adminKek, dek);
            CryptographicOperations.ZeroMemory(adminKek);

            await _users.CreateAsync(new NewUser(
                Username: username.Trim(),
                FullName: fullName.Trim(),
                Role: UserRole.Admin,
                PasswordHash: _hasher.Hash(password),
                KdfSalt: adminSalt,
                WrappedDek: adminWrapped,
                IsActive: true,
                MustChangePassword: false), ct);

            // --- Recovery-code-wrapped copy of the DEK ---
            string recoveryCode = _recovery.Generate();
            byte[] recoverySalt = _kdf.NewSalt();
            byte[] recoveryKek = _kdf.DeriveKey(_recovery.Normalize(recoveryCode), recoverySalt);
            byte[] recoveryWrapped = _crypto.WrapKey(recoveryKek, dek);
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
