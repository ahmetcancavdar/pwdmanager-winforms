using System.Security.Cryptography;

namespace PwdManager.Core.Cryptography;

/// <summary>
/// The Data Encryption Key (DEK): a single random 256-bit key that encrypts every
/// stored secret. It is never persisted in the clear — only wrapped (encrypted)
/// with a KEK derived from each user's password, and once with the recovery code.
/// </summary>
public static class DataKey
{
    public const int Size = 32;

    public static byte[] Generate() => RandomNumberGenerator.GetBytes(Size);

    /// <summary>wrapped_dek = AES-256-GCM(kek, dek).</summary>
    public static byte[] Wrap(byte[] kek, byte[] dek)
    {
        if (dek.Length != Size)
            throw new ArgumentException($"DEK must be {Size} bytes.", nameof(dek));
        return AeadCipher.Encrypt(kek, dek);
    }

    /// <summary>Returns the DEK, or throws <see cref="CryptographicException"/> if the KEK is wrong.</summary>
    public static byte[] Unwrap(byte[] kek, byte[] wrappedDek)
    {
        byte[] dek = AeadCipher.Decrypt(kek, wrappedDek);
        if (dek.Length != Size)
        {
            CryptographicOperations.ZeroMemory(dek);
            throw new CryptographicException("Unwrapped DEK has an unexpected length.");
        }
        return dek;
    }
}
