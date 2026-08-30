using System.Security.Cryptography;
using System.Text;

namespace PwdManager.Core.Cryptography;

/// <summary>
/// AES-256-GCM authenticated encryption. Every blob is self-contained:
/// <c>nonce(12) || ciphertext(n) || tag(16)</c>. A fresh random nonce is used
/// for every call, so the same plaintext never produces the same blob.
/// </summary>
public static class AeadCipher
{
    public const int KeySize = 32;
    public const int NonceSize = 12;
    public const int TagSize = 16;

    public static byte[] Encrypt(byte[] key, ReadOnlySpan<byte> plaintext)
    {
        if (key.Length != KeySize)
            throw new ArgumentException($"Key must be {KeySize} bytes.", nameof(key));

        byte[] nonce = RandomNumberGenerator.GetBytes(NonceSize);
        byte[] blob = new byte[NonceSize + plaintext.Length + TagSize];

        Buffer.BlockCopy(nonce, 0, blob, 0, NonceSize);
        Span<byte> ciphertext = blob.AsSpan(NonceSize, plaintext.Length);
        Span<byte> tag = blob.AsSpan(NonceSize + plaintext.Length, TagSize);

        using var gcm = new AesGcm(key, TagSize);
        gcm.Encrypt(nonce, plaintext, ciphertext, tag);
        return blob;
    }

    public static byte[] Decrypt(byte[] key, ReadOnlySpan<byte> blob)
    {
        if (key.Length != KeySize)
            throw new ArgumentException($"Key must be {KeySize} bytes.", nameof(key));
        if (blob.Length < NonceSize + TagSize)
            throw new CryptographicException("Ciphertext blob is too short.");

        ReadOnlySpan<byte> nonce = blob[..NonceSize];
        ReadOnlySpan<byte> tag = blob[^TagSize..];
        ReadOnlySpan<byte> ciphertext = blob[NonceSize..^TagSize];

        byte[] plaintext = new byte[ciphertext.Length];
        using var gcm = new AesGcm(key, TagSize);
        gcm.Decrypt(nonce, ciphertext, tag, plaintext); // throws CryptographicException on tamper / wrong key
        return plaintext;
    }

    public static byte[] EncryptString(byte[] key, string value) =>
        Encrypt(key, Encoding.UTF8.GetBytes(value));

    public static string DecryptString(byte[] key, ReadOnlySpan<byte> blob) =>
        Encoding.UTF8.GetString(Decrypt(key, blob));
}
