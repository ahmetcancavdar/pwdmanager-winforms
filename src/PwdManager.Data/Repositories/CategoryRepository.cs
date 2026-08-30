using Microsoft.EntityFrameworkCore;
using PwdManager.Data.Entities;
using PwdManager.Data.Persistence;

namespace PwdManager.Data.Repositories;

public sealed class CategoryRepository : ICategoryRepository
{
    private readonly IDbContextFactory<PwdManagerContext> _factory;

    public CategoryRepository(IDbContextFactory<PwdManagerContext> factory) => _factory = factory;

    public async Task<IReadOnlyList<Category>> ListAsync(CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.Categories.AsNoTracking().OrderBy(c => c.Name).ToListAsync(ct);
    }

    public async Task<Category?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<long> CreateAsync(string name, string description, long createdBy, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var category = new Category { Name = name, Description = description, CreatedBy = createdBy };
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

    public async Task DeleteAsync(long id, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        await db.Categories.Where(c => c.Id == id).ExecuteDeleteAsync(ct);
    }

    public async Task<int> CountSecretsAsync(long categoryId, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.Secrets.CountAsync(s => s.CategoryId == categoryId, ct);
    }

    public async Task<bool> NameExistsAsync(string name, long? excludingId = null, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.Categories.AsNoTracking()
            .AnyAsync(c => c.Name == name && (excludingId == null || c.Id != excludingId), ct);
    }
}
