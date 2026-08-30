namespace PwdManager.Data.Repositories;

/// <summary>
/// Access grants and the per-user sync counter. Every grant/revoke bumps the
/// counter so the personnel client refreshes within its poll interval.
/// </summary>
public interface IPermissionRepository
{
    Task<IReadOnlyList<long>> GetGrantedCategoryIdsAsync(long userId, CancellationToken ct = default);
    Task<IReadOnlyList<long>> GetGrantedSecretIdsAsync(long userId, CancellationToken ct = default);

    Task GrantCategoryAsync(long userId, long categoryId, long grantedBy, CancellationToken ct = default);
    Task RevokeCategoryAsync(long userId, long categoryId, CancellationToken ct = default);
    Task GrantSecretAsync(long userId, long secretId, long grantedBy, CancellationToken ct = default);
    Task RevokeSecretAsync(long userId, long secretId, CancellationToken ct = default);

    /// <summary>Effective visible secrets = granted categories ∪ individually granted secrets.</summary>
    Task<IReadOnlyList<SecretSummary>> ListVisibleSecretsAsync(long userId, CancellationToken ct = default);

    /// <summary>Authoritative re-check performed at reveal time.</summary>
    Task<bool> CanViewSecretAsync(long userId, long secretId, CancellationToken ct = default);

    Task<long> GetSyncVersionAsync(long userId, CancellationToken ct = default);
    Task BumpSyncVersionAsync(long userId, CancellationToken ct = default);
}
