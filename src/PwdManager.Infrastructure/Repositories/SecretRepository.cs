using Microsoft.EntityFrameworkCore;
using PwdManager.Application.Interfaces;
using PwdManager.Application.Models;
using PwdManager.Infrastructure.Persistence;

namespace PwdManager.Infrastructure.Repositories;

public sealed class SecretRepository : ISecretRepository
{
    private readonly IDbContextFactory<PwdManagerContext> _factory;

    public SecretRepository(IDbContextFactory<PwdManagerContext> factory) => _factory = factory;

    public async Task<IReadOnlyList<SecretSummary>> ListAllSummariesAsync(CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.Secrets.AsNoTracking()
            .Where(s => s.DeletedAt == null && s.Category.DeletedAt == null)
            .OrderBy(s => s.Category.Name).ThenBy(s => s.Title)
            .Select(s => new SecretSummary(s.Id, s.CategoryId, s.Category.Name, s.Title, s.Notes, s.UpdatedAt))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<SecretSummary>> ListSummariesByCategoryAsync(long categoryId, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.Secrets.AsNoTracking()
            .Where(s => s.CategoryId == categoryId && s.DeletedAt == null && s.Category.DeletedAt == null)
            .OrderBy(s => s.Title)
            .Select(s => new SecretSummary(s.Id, s.CategoryId, s.Category.Name, s.Title, s.Notes, s.UpdatedAt))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<SecretSummary>> ListDeletedAsync(CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        // Secrets deleted on their own, whose category is still active. Secrets hidden only
        // because their category is deleted come back with the category, so they are not
        // listed separately here.
        return await db.Secrets.AsNoTracking()
            .Where(s => s.DeletedAt != null && s.Category.DeletedAt == null)
            .OrderByDescending(s => s.DeletedAt)
            .Select(s => new SecretSummary(s.Id, s.CategoryId, s.Category.Name, s.Title, s.Notes, s.DeletedAt!.Value))
            .ToListAsync(ct);
    }

    public async Task<SecretRecord?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var e = await db.Secrets.AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null && s.Category.DeletedAt == null, ct);
        return e?.ToRecord();
    }

    public async Task<SecretRecord?> GetByIdRawAsync(long id, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var e = await db.Secrets.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, ct);
        return e?.ToRecord();
    }

    public async Task<long> GetCategoryIdAsync(long secretId, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.Secrets.AsNoTracking()
            .Where(s => s.Id == secretId)
            .Select(s => s.CategoryId)
            .FirstOrDefaultAsync(ct);
    }

    public async Task<long> CreateAsync(NewSecret secret, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var e = new Entities.Secret
        {
            CategoryId = secret.CategoryId,
            Title = secret.Title,
            UsernameCipher = secret.UsernameCipher,
            SecretCipher = secret.SecretCipher,
            Notes = secret.Notes,
            CreatedBy = secret.CreatedBy
        };
        db.Secrets.Add(e);
        await db.SaveChangesAsync(ct);
        return e.Id;
    }

    public async Task UpdateAsync(long id, long categoryId, string title, byte[]? usernameCipher, byte[] secretCipher,
        string notes, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var existing = await db.Secrets.FirstOrDefaultAsync(s => s.Id == id && s.DeletedAt == null, ct)
                       ?? throw new InvalidOperationException($"Secret {id} not found.");
        existing.CategoryId = categoryId;
        existing.Title = title;
        existing.UsernameCipher = usernameCipher;
        existing.SecretCipher = secretCipher;
        existing.Notes = notes;
        existing.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
    }

    public async Task SoftDeleteAsync(long id, long deletedBy, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        await db.Secrets
            .Where(s => s.Id == id && s.DeletedAt == null)
            .ExecuteUpdateAsync(x => x
                .SetProperty(s => s.DeletedAt, DateTime.UtcNow)
                .SetProperty(s => s.DeletedBy, deletedBy), ct);
    }

    public async Task RestoreAsync(long id, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        await db.Secrets
            .Where(s => s.Id == id)
            .ExecuteUpdateAsync(x => x
                .SetProperty(s => s.DeletedAt, (DateTime?)null)
                .SetProperty(s => s.DeletedBy, (long?)null), ct);
    }

    public async Task PurgeAsync(long id, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        await db.Secrets.Where(s => s.Id == id).ExecuteDeleteAsync(ct);
    }
}
