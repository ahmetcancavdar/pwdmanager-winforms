namespace PwdManager.Application.Interfaces;

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
