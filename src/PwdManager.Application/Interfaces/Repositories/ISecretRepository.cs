using PwdManager.Application.DTOs;
using PwdManager.Domain.Entities;

namespace PwdManager.Application.Interfaces;

public interface ISecretRepository
{
    Task<IReadOnlyList<SecretSummary>> ListAllSummariesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<SecretSummary>> ListSummariesByCategoryAsync(long categoryId, CancellationToken ct = default);
    Task<IReadOnlyList<SecretSummary>> ListDeletedAsync(CancellationToken ct = default);

    Task<SecretRecord?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<SecretRecord?> GetByIdRawAsync(long id, CancellationToken ct = default);
    Task<long> GetCategoryIdAsync(long secretId, CancellationToken ct = default);

    Task<long> CreateAsync(NewSecret secret, CancellationToken ct = default);
    Task UpdateAsync(long id, long categoryId, string title, byte[]? usernameCipher, byte[] secretCipher,
        string notes, CancellationToken ct = default);

    Task SoftDeleteAsync(long id, long deletedBy, CancellationToken ct = default);
    Task RestoreAsync(long id, CancellationToken ct = default);
    Task PurgeAsync(long id, CancellationToken ct = default);
}
