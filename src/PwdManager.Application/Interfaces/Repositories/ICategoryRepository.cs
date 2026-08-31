using PwdManager.Domain.Entities;

namespace PwdManager.Application.Interfaces;

public interface ICategoryRepository
{
    Task<IReadOnlyList<CategoryRecord>> ListAsync(CancellationToken ct = default);
    Task<IReadOnlyList<CategoryRecord>> ListDeletedAsync(CancellationToken ct = default);
    Task<CategoryRecord?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<CategoryRecord?> GetByIdRawAsync(long id, CancellationToken ct = default);

    Task<long> CreateAsync(string name, string description, long createdBy, CancellationToken ct = default);
    Task UpdateAsync(long id, string name, string description, CancellationToken ct = default);

    Task SoftDeleteAsync(long id, long deletedBy, CancellationToken ct = default);
    Task RestoreAsync(long id, CancellationToken ct = default);
    Task PurgeAsync(long id, CancellationToken ct = default);

    Task<int> CountActiveSecretsAsync(long categoryId, CancellationToken ct = default);
    Task<bool> ActiveNameExistsAsync(string name, long? excludingId = null, CancellationToken ct = default);
}
