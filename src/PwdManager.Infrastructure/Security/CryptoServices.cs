using PwdManager.Domain.Security;

namespace PwdManager.Infrastructure.Security;

/// <summary>Argon2id-backed <see cref="IPasswordHasher"/>.</summary>
public sealed class Argon2PasswordHasher : IPasswordHasher
{
    public string Hash(string password) => PasswordHasher.Hash(password);
    public bool Verify(string password, string encoded) => PasswordHasher.Verify(password, encoded);
}

/// <summary>Argon2id-backed <see cref="IKeyDerivation"/>.</summary>
public sealed class Argon2KeyDerivation : IKeyDerivation
{
    public byte[] NewSalt() => KeyDerivation.NewSalt();
    public byte[] DeriveKey(string password, byte[] salt) => KeyDerivation.DeriveKek(password, salt);
}

/// <summary>AES-256-GCM-backed <see cref="IDataProtector"/> (DEK generate/wrap/unwrap + secret AEAD).</summary>
public sealed class AesGcmDataProtector : IDataProtector
{
    public byte[] NewDataKey() => DataKey.Generate();
    public byte[] WrapKey(byte[] kek, byte[] dataKey) => DataKey.Wrap(kek, dataKey);
    public byte[] UnwrapKey(byte[] kek, byte[] wrapped) => DataKey.Unwrap(kek, wrapped);
    public byte[] Protect(byte[] key, ReadOnlySpan<byte> plaintext) => AeadCipher.Encrypt(key, plaintext);
    public byte[] Unprotect(byte[] key, ReadOnlySpan<byte> blob) => AeadCipher.Decrypt(key, blob);
}

/// <summary>Crockford-style recovery code (<see cref="IRecoveryCodeService"/>).</summary>
public sealed class RecoveryCodeService : IRecoveryCodeService
{
    public string Generate() => RecoveryCode.Generate();
    public string Normalize(string input) => RecoveryCode.Normalize(input);
}
