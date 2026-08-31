using PwdManager.Application.DTOs;

namespace PwdManager.Application.Interfaces;

/// <summary>
/// Access grants and the per-user sync counter. Effective visibility of a secret =
/// (category granted OR secret granted) AND NOT denied AND not soft-deleted.
/// </summary>
public interface IPermissionRepository
{
    Task<IReadOnlyList<long>> GetGrantedCategoryIdsAsync(long userId, CancellationToken ct = default);
    Task<IReadOnlyList<long>> GetGrantedSecretIdsAsync(long userId, CancellationToken ct = default);
    Task<IReadOnlyList<long>> GetDeniedSecretIdsAsync(long userId, CancellationToken ct = default);

    Task GrantCategoryAsync(long userId, long categoryId, long grantedBy, CancellationToken ct = default);
    Task RevokeCategoryAsync(long userId, long categoryId, CancellationToken ct = default);
    Task GrantSecretAsync(long userId, long secretId, long grantedBy, CancellationToken ct = default);
    Task RevokeSecretAsync(long userId, long secretId, CancellationToken ct = default);

    Task DenySecretAsync(long userId, long secretId, long deniedBy, CancellationToken ct = default);
    Task UndenySecretAsync(long userId, long secretId, CancellationToken ct = default);

    Task<IReadOnlyList<SecretSummary>> ListVisibleSecretsAsync(long userId, CancellationToken ct = default);
    Task<bool> CanViewSecretAsync(long userId, long secretId, CancellationToken ct = default);

    Task<long> GetSyncVersionAsync(long userId, CancellationToken ct = default);
    Task BumpSyncVersionAsync(long userId, CancellationToken ct = default);
}
