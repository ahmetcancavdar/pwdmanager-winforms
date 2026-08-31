using PwdManager.Application.Interfaces;
using PwdManager.Application.Security;

namespace PwdManager.Application.Services;

/// <summary>
/// Admin-only "Silinenler" (çöp kutusu): soft-deleted kategori ve parolaları listeler,
/// geri yükler veya kalıcı olarak siler. Geri yükleme, veriyi tam olarak silinmeden
/// önceki haline döndürür (yetkiler/istisnalar zaten satır olarak duruyordu).
/// </summary>
public sealed class TrashService
{
    public enum ItemKind { Category, Secret }

    public sealed record TrashItem(
        ItemKind Kind,
        long Id,
        string Name,
        string CategoryName,
        DateTime DeletedAt);

    private readonly ICategoryRepository _categories;
    private readonly ISecretRepository _secrets;
    private readonly IAuditRepository _audit;

    public TrashService(ICategoryRepository categories, ISecretRepository secrets, IAuditRepository audit)
    {
        _categories = categories;
        _secrets = secrets;
        _audit = audit;
    }

    public async Task<IReadOnlyList<TrashItem>> ListAsync(SessionContext admin, CancellationToken ct = default)
    {
        admin.EnsureAdmin();

        var items = new List<TrashItem>();

        foreach (var c in await _categories.ListDeletedAsync(ct))
            items.Add(new TrashItem(ItemKind.Category, c.Id, c.Name, "—", c.DeletedAt ?? DateTime.MinValue));

        foreach (var s in await _secrets.ListDeletedAsync(ct))
            items.Add(new TrashItem(ItemKind.Secret, s.Id, s.Title, s.CategoryName, s.UpdatedAt));

        return items.OrderByDescending(i => i.DeletedAt).ToList();
    }

    // ---------------------------------------------------------------- restore
    public async Task RestoreCategoryAsync(SessionContext admin, long id, CancellationToken ct = default)
    {
        admin.EnsureAdmin();

        var category = await _categories.GetByIdRawAsync(id, ct)
                       ?? throw new InvalidOperationException("Kategori bulunamadı.");
        if (category.DeletedAt is null)
            return; // already active

        if (await _categories.ActiveNameExistsAsync(category.Name, id, ct))
            throw new InvalidOperationException(
                $"'{category.Name}' adında aktif bir kategori var. Önce onu yeniden adlandır, sonra geri yükle.");

        await _categories.RestoreAsync(id, ct);
        await _audit.WriteAsync(AuditAction.CategoryRestore, admin.User.Id, admin.User.Username, "category", id, category.Name, ct);
    }

    public async Task RestoreSecretAsync(SessionContext admin, long id, CancellationToken ct = default)
    {
        admin.EnsureAdmin();

        var secret = await _secrets.GetByIdRawAsync(id, ct)
                     ?? throw new InvalidOperationException("Kayıt bulunamadı.");
        if (secret.DeletedAt is null)
            return;

        var category = await _categories.GetByIdRawAsync(secret.CategoryId, ct);
        if (category is null || category.DeletedAt is not null)
            throw new InvalidOperationException("Bu parolanın kategorisi de silinmiş. Önce kategoriyi geri yükle.");

        await _secrets.RestoreAsync(id, ct);
        await _audit.WriteAsync(AuditAction.SecretRestore, admin.User.Id, admin.User.Username, "secret", id, secret.Title, ct);
    }

    // ---------------------------------------------------------------- permanent delete
    public async Task PurgeCategoryAsync(SessionContext admin, long id, CancellationToken ct = default)
    {
        admin.EnsureAdmin();
        var category = await _categories.GetByIdRawAsync(id, ct);
        if (category is null) return;
        if (category.DeletedAt is null)
            throw new InvalidOperationException("Yalnızca çöp kutusundaki kayıt kalıcı silinebilir.");

        await _categories.PurgeAsync(id, ct); // FK cascade: secrets + permissions + denies
        await _audit.WriteAsync(AuditAction.CategoryPurge, admin.User.Id, admin.User.Username, "category", id, category.Name, ct);
    }

    public async Task PurgeSecretAsync(SessionContext admin, long id, CancellationToken ct = default)
    {
        admin.EnsureAdmin();
        var secret = await _secrets.GetByIdRawAsync(id, ct);
        if (secret is null) return;
        if (secret.DeletedAt is null)
            throw new InvalidOperationException("Yalnızca çöp kutusundaki kayıt kalıcı silinebilir.");

        await _secrets.PurgeAsync(id, ct);
        await _audit.WriteAsync(AuditAction.SecretPurge, admin.User.Id, admin.User.Username, "secret", id, secret.Title, ct);
    }
}
