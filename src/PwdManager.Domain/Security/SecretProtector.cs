using System.Text;

namespace PwdManager.Domain.Security;

/// <summary>
/// Encrypts/decrypts secret fields with a DEK held for the lifetime of a session.
/// The raw DEK is not exposed; callers only get protect/unprotect operations.
/// </summary>
public sealed class SecretProtector
{
    private readonly byte[] _dek;
    private readonly IDataProtector _crypto;

    public SecretProtector(byte[] dek, IDataProtector crypto)
    {
        _dek = (byte[])dek.Clone();
        _crypto = crypto;
    }

    public byte[] Protect(string plaintext) => _crypto.Protect(_dek, Encoding.UTF8.GetBytes(plaintext));

    public string Unprotect(ReadOnlySpan<byte> blob) => Encoding.UTF8.GetString(_crypto.Unprotect(_dek, blob));

    /// <summary>
    /// Produces a wrapped copy of the DEK under the supplied KEK, without exposing the
    /// DEK itself. Used when an admin provisions another user (their password-derived KEK).
    /// </summary>
    public byte[] RewrapDekUnder(byte[] kek) => _crypto.WrapKey(kek, _dek);

    /// <summary>Zero the DEK copy held here. Call when the session ends.</summary>
    public void Clear() => System.Security.Cryptography.CryptographicOperations.ZeroMemory(_dek);
}
