using PwdManager.Core.Cryptography;

namespace PwdManager.Core.Security;

/// <summary>
/// Encrypts/decrypts secret fields with a DEK held for the lifetime of a session.
/// The raw DEK is not exposed; callers only get protect/unprotect operations.
/// </summary>
public sealed class SecretProtector
{
    private readonly byte[] _dek;

    public SecretProtector(byte[] dek)
    {
        if (dek.Length != DataKey.Size)
            throw new ArgumentException("Invalid DEK length.", nameof(dek));
        _dek = (byte[])dek.Clone();
    }

    public byte[] Protect(string plaintext) => AeadCipher.EncryptString(_dek, plaintext);

    public string Unprotect(ReadOnlySpan<byte> blob) => AeadCipher.DecryptString(_dek, blob);

    /// <summary>
    /// Produces a wrapped copy of the DEK under the supplied KEK, without exposing the
    /// DEK itself. Used when an admin provisions another user (their password-derived KEK).
    /// </summary>
    public byte[] RewrapDekUnder(byte[] kek) => DataKey.Wrap(kek, _dek);

    /// <summary>Zero the DEK copy held here. Call when the session ends.</summary>
    public void Clear() => System.Security.Cryptography.CryptographicOperations.ZeroMemory(_dek);
}
