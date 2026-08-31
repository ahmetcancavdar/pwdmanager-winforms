namespace PwdManager.Application.Interfaces;

/// <summary>Small binary key/value store (schema version, recovery salt, recovery-wrapped DEK).</summary>
public interface IAppMetaRepository
{
    Task<byte[]?> GetAsync(string key, CancellationToken ct = default);
    Task SetAsync(string key, byte[] value, CancellationToken ct = default);
}
