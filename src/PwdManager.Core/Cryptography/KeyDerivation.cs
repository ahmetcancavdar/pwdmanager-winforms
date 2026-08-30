using System.Security.Cryptography;

namespace PwdManager.Core.Cryptography;

/// <summary>
/// Derives the per-user Key Encryption Key (KEK) from a login password and the
/// user's stored <c>kdf_salt</c>. The KEK never leaves memory; it only wraps/unwraps
/// the Data Encryption Key (DEK).
/// </summary>
public static class KeyDerivation
{
    public const int KekSize = 32;   // AES-256
    public const int SaltSize = 16;

    public static byte[] NewSalt() => RandomNumberGenerator.GetBytes(SaltSize);

    public static byte[] DeriveKek(string password, byte[] salt) =>
        Argon2Kdf.Derive(password, salt,
            Argon2Kdf.DefaultMemoryKib, Argon2Kdf.DefaultIterations, Argon2Kdf.DefaultParallelism, KekSize);
}
