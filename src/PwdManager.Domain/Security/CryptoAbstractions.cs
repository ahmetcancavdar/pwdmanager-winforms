namespace PwdManager.Domain.Security;

/// <summary>
/// Login-password hashing. The concrete algorithm (Argon2id) lives in Infrastructure;
/// Application/Domain only see "hash" and "verify".
/// </summary>
public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string encoded);
}

/// <summary>Derives a 256-bit key-encryption key (KEK) from a password + salt.</summary>
public interface IKeyDerivation
{
    byte[] NewSalt();
    byte[] DeriveKey(string password, byte[] salt);
}

/// <summary>
/// The AEAD box: generate/wrap/unwrap the data-encryption key (DEK) and
/// protect/unprotect secret bytes under a key. Concrete impl = AES-256-GCM.
/// </summary>
public interface IDataProtector
{
    byte[] NewDataKey();
    byte[] WrapKey(byte[] kek, byte[] dataKey);
    byte[] UnwrapKey(byte[] kek, byte[] wrapped);
    byte[] Protect(byte[] key, ReadOnlySpan<byte> plaintext);
    byte[] Unprotect(byte[] key, ReadOnlySpan<byte> blob);
}

/// <summary>Human-transcribable recovery code generation + normalisation.</summary>
public interface IRecoveryCodeService
{
    string Generate();
    string Normalize(string input);
}
