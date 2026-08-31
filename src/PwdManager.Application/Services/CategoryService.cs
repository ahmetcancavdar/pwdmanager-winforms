using PwdManager.Application.Interfaces;
using PwdManager.Application.Models;
using PwdManager.Domain.Security;

namespace PwdManager.Application.Services;

public sealed class CategoryService
{
    private readonly ICategoryRepository _categories;
    private readonly IAuditRepository _audit;

    public CategoryService(ICategoryRepository categories, IAuditRepository audit)
    {
        _categories = categories;
        _audit = audit;
    }

    public Task<IReadOnlyList<CategoryRecord>> ListAsync(CancellationToken ct = default) => _categories.ListAsync(ct);

    public Task<int> CountActiveSecretsAsync(long categoryId, CancellationToken ct = default) =>
        _categories.CountActiveSecretsAsync(categoryId, ct);

    public async Task<long> CreateAsync(SessionContext admin, string name, string description, CancellationToken ct = default)
    {
        admin.EnsureAdmin();
        name = name.Trim();
        if (name.Length == 0) throw new InvalidOperationException("Kategori adı boş olamaz.");
        if (await _categories.ActiveNameExistsAsync(name, null, ct))
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
        if (await _categories.ActiveNameExistsAsync(name, id, ct))
            throw new InvalidOperationException("Bu adda başka bir kategori zaten var.");

        await _categories.UpdateAsync(id, name, description.Trim(), ct);
        await _audit.WriteAsync(AuditAction.CategoryEdit, admin.User.Id, admin.User.Username, "category", id, name, ct);
    }

    /// <summary>Soft delete → "Silinenler"e taşır; kategori ve parolaları DB'de kalır.</summary>
    public async Task DeleteAsync(SessionContext admin, long id, string name, CancellationToken ct = default)
    {
        admin.EnsureAdmin();
        await _categories.SoftDeleteAsync(id, admin.User.Id, ct);
        await _audit.WriteAsync(AuditAction.CategoryDelete, admin.User.Id, admin.User.Username, "category", id, name, ct);
    }
}
