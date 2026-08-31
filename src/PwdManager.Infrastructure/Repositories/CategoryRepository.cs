using Microsoft.EntityFrameworkCore;
using PwdManager.Application.Interfaces;
using PwdManager.Application.Models;
using PwdManager.Infrastructure.Persistence;

namespace PwdManager.Infrastructure.Repositories;

public sealed class CategoryRepository : ICategoryRepository
{
    private readonly IDbContextFactory<PwdManagerContext> _factory;

    public CategoryRepository(IDbContextFactory<PwdManagerContext> factory) => _factory = factory;

    public async Task<IReadOnlyList<CategoryRecord>> ListAsync(CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var rows = await db.Categories.AsNoTracking()
            .Where(c => c.DeletedAt == null).OrderBy(c => c.Name).ToListAsync(ct);
        return rows.Select(Map.ToRecord).ToList();
    }

    public async Task<IReadOnlyList<CategoryRecord>> ListDeletedAsync(CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var rows = await db.Categories.AsNoTracking()
            .Where(c => c.DeletedAt != null).OrderByDescending(c => c.DeletedAt).ToListAsync(ct);
        return rows.Select(Map.ToRecord).ToList();
    }

    public async Task<CategoryRecord?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var e = await db.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null, ct);
        return e?.ToRecord();
    }

    public async Task<CategoryRecord?> GetByIdRawAsync(long id, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var e = await db.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, ct);
        return e?.ToRecord();
    }

    public async Task<long> CreateAsync(string name, string description, long createdBy, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var category = new Entities.Category { Name = name, Description = description, CreatedBy = createdBy };
        db.Categories.Add(category);
        await db.SaveChangesAsync(ct);
        return category.Id;
    }

    public async Task UpdateAsync(long id, string name, string description, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var category = await db.Categories.FirstOrDefaultAsync(c => c.Id == id, ct)
                       ?? throw new InvalidOperationException($"Category {id} not found.");
        category.Name = name;
        category.Description = description;
        await db.SaveChangesAsync(ct);
    }

    public async Task SoftDeleteAsync(long id, long deletedBy, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        await db.Categories
            .Where(c => c.Id == id && c.DeletedAt == null)
            .ExecuteUpdateAsync(s => s
                .SetProperty(c => c.DeletedAt, DateTime.UtcNow)
                .SetProperty(c => c.DeletedBy, deletedBy), ct);
    }

    public async Task RestoreAsync(long id, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        await db.Categories
            .Where(c => c.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(c => c.DeletedAt, (DateTime?)null)
                .SetProperty(c => c.DeletedBy, (long?)null), ct);
    }

    public async Task PurgeAsync(long id, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        await db.Categories.Where(c => c.Id == id).ExecuteDeleteAsync(ct);
    }

    public async Task<int> CountActiveSecretsAsync(long categoryId, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.Secrets.CountAsync(s => s.CategoryId == categoryId && s.DeletedAt == null, ct);
    }

    public async Task<bool> ActiveNameExistsAsync(string name, long? excludingId = null, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.Categories.AsNoTracking()
            .AnyAsync(c => c.DeletedAt == null && c.Name == name && (excludingId == null || c.Id != excludingId), ct);
    }
}
