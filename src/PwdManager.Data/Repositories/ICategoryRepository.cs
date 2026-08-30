using PwdManager.Data.Entities;

namespace PwdManager.Data.Repositories;

public interface ICategoryRepository
{
    Task<IReadOnlyList<Category>> ListAsync(CancellationToken ct = default);
    Task<Category?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<long> CreateAsync(string name, string description, long createdBy, CancellationToken ct = default);
    Task UpdateAsync(long id, string name, string description, CancellationToken ct = default);

    /// <summary>Deletes the category; FK cascade removes its secrets and related grants.</summary>
    Task DeleteAsync(long id, CancellationToken ct = default);

    Task<int> CountSecretsAsync(long categoryId, CancellationToken ct = default);
    Task<bool> NameExistsAsync(string name, long? excludingId = null, CancellationToken ct = default);
}
