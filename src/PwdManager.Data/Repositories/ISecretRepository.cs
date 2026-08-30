using PwdManager.Data.Entities;

namespace PwdManager.Data.Repositories;

public interface ISecretRepository
{
    /// <summary>All secrets (admin view), no cipher material.</summary>
    Task<IReadOnlyList<SecretSummary>> ListAllSummariesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<SecretSummary>> ListSummariesByCategoryAsync(long categoryId, CancellationToken ct = default);

    /// <summary>Full row including cipher blobs — for admin edit or a permitted reveal.</summary>
    Task<Secret?> GetByIdAsync(long id, CancellationToken ct = default);

    Task<long> CreateAsync(Secret secret, CancellationToken ct = default);
    Task UpdateAsync(Secret secret, CancellationToken ct = default);
    Task DeleteAsync(long id, CancellationToken ct = default);
}
