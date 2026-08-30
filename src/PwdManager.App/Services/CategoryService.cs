using PwdManager.Core.Security;
using PwdManager.Data.Entities;
using PwdManager.Data.Repositories;

namespace PwdManager.App.Services;

public sealed class CategoryService
{
    private readonly ICategoryRepository _categories;
    private readonly IAuditRepository _audit;

    public CategoryService(ICategoryRepository categories, IAuditRepository audit)
    {
        _categories = categories;
        _audit = audit;
    }

    public Task<IReadOnlyList<Category>> ListAsync(CancellationToken ct = default) => _categories.ListAsync(ct);

    public Task<int> CountSecretsAsync(long categoryId, CancellationToken ct = default) =>
        _categories.CountSecretsAsync(categoryId, ct);

    public async Task<long> CreateAsync(SessionContext admin, string name, string description, CancellationToken ct = default)
    {
        admin.EnsureAdmin();
        name = name.Trim();
        if (name.Length == 0) throw new InvalidOperationException("Kategori adı boş olamaz.");
        if (await _categories.NameExistsAsync(name, null, ct))
            throw new InvalidOperationException("Bu adda bir kategori zaten var.");

        long id = await _categories.CreateAsync(name, description.Trim(), admin.User.Id, ct);
        await _audit.WriteAsync(AuditAction.CategoryAdd, admin.User.Id, admin.User.Username, "category", id, name, ct);
        return id;
    }

    public async Task UpdateAsync(SessionContext admin, long id, string name, string description, CancellationToken ct = default)
    {
        admin.EnsureAdmin();
        name = name.Trim();
        if (name.Length == 0) throw new InvalidOperationException("Kategori adı boş olamaz.");
        if (await _categories.NameExistsAsync(name, id, ct))
            throw new InvalidOperationException("Bu adda başka bir kategori zaten var.");

        await _categories.UpdateAsync(id, name, description.Trim(), ct);
        await _audit.WriteAsync(AuditAction.CategoryEdit, admin.User.Id, admin.User.Username, "category", id, name, ct);
    }

    public async Task DeleteAsync(SessionContext admin, long id, string name, CancellationToken ct = default)
    {
        admin.EnsureAdmin();
        await _categories.DeleteAsync(id, ct);
        await _audit.WriteAsync(AuditAction.CategoryDelete, admin.User.Id, admin.User.Username, "category", id, name, ct);
    }
}
