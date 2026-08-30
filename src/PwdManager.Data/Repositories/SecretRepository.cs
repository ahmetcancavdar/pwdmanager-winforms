using Microsoft.EntityFrameworkCore;
using PwdManager.Data.Entities;
using PwdManager.Data.Persistence;

namespace PwdManager.Data.Repositories;

public sealed class SecretRepository : ISecretRepository
{
    private readonly IDbContextFactory<PwdManagerContext> _factory;

    public SecretRepository(IDbContextFactory<PwdManagerContext> factory) => _factory = factory;

    public async Task<IReadOnlyList<SecretSummary>> ListAllSummariesAsync(CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.Secrets.AsNoTracking()
            .OrderBy(s => s.Category.Name).ThenBy(s => s.Title)
            .Select(s => new SecretSummary(s.Id, s.CategoryId, s.Category.Name, s.Title, s.Notes, s.UpdatedAt))
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<SecretSummary>> ListSummariesByCategoryAsync(long categoryId, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.Secrets.AsNoTracking()
            .Where(s => s.CategoryId == categoryId)
            .OrderBy(s => s.Title)
            .Select(s => new SecretSummary(s.Id, s.CategoryId, s.Category.Name, s.Title, s.Notes, s.UpdatedAt))
            .ToListAsync(ct);
    }

    public async Task<Secret?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.Secrets.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task<long> CreateAsync(Secret secret, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        db.Secrets.Add(secret);
        await db.SaveChangesAsync(ct);
        return secret.Id;
    }

    public async Task UpdateAsync(Secret secret, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var existing = await db.Secrets.FirstOrDefaultAsync(s => s.Id == secret.Id, ct)
                       ?? throw new InvalidOperationException($"Secret {secret.Id} not found.");
        existing.CategoryId = secret.CategoryId;
        existing.Title = secret.Title;
        existing.UsernameCipher = secret.UsernameCipher;
        existing.SecretCipher = secret.SecretCipher;
        existing.Notes = secret.Notes;
        await db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        await db.Secrets.Where(s => s.Id == id).ExecuteDeleteAsync(ct);
    }
}
