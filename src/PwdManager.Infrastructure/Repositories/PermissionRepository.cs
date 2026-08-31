using Microsoft.EntityFrameworkCore;
using PwdManager.Application.Interfaces;
using PwdManager.Application.DTOs;
using PwdManager.Infrastructure.Persistence.Entities;
using PwdManager.Infrastructure.Persistence;

namespace PwdManager.Infrastructure.Repositories;

public sealed class PermissionRepository : IPermissionRepository
{
    private readonly IDbContextFactory<PwdManagerContext> _factory;

    public PermissionRepository(IDbContextFactory<PwdManagerContext> factory) => _factory = factory;

    public async Task<IReadOnlyList<long>> GetGrantedCategoryIdsAsync(long userId, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.CategoryPermissions.AsNoTracking()
            .Where(cp => cp.UserId == userId).Select(cp => cp.CategoryId).ToListAsync(ct);
    }

    public async Task<IReadOnlyList<long>> GetGrantedSecretIdsAsync(long userId, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.SecretPermissions.AsNoTracking()
            .Where(sp => sp.UserId == userId).Select(sp => sp.SecretId).ToListAsync(ct);
    }

    public async Task<IReadOnlyList<long>> GetDeniedSecretIdsAsync(long userId, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.SecretDenies.AsNoTracking()
            .Where(sd => sd.UserId == userId).Select(sd => sd.SecretId).ToListAsync(ct);
    }

    public async Task GrantCategoryAsync(long userId, long categoryId, long grantedBy, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        bool exists = await db.CategoryPermissions
            .AnyAsync(cp => cp.UserId == userId && cp.CategoryId == categoryId, ct);
        if (!exists)
        {
            db.CategoryPermissions.Add(new CategoryPermission
            {
                UserId = userId,
                CategoryId = categoryId,
                GrantedBy = grantedBy
            });
        }

        // A fresh full-category grant clears any earlier carve-outs for that category.
        await db.SecretDenies
            .Where(sd => sd.UserId == userId &&
                         db.Secrets.Any(s => s.Id == sd.SecretId && s.CategoryId == categoryId))
            .ExecuteDeleteAsync(ct);

        await BumpAsync(db, userId, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task RevokeCategoryAsync(long userId, long categoryId, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        await db.CategoryPermissions
            .Where(cp => cp.UserId == userId && cp.CategoryId == categoryId)
            .ExecuteDeleteAsync(ct);
        await BumpAsync(db, userId, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task GrantSecretAsync(long userId, long secretId, long grantedBy, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        bool exists = await db.SecretPermissions
            .AnyAsync(sp => sp.UserId == userId && sp.SecretId == secretId, ct);
        if (!exists)
        {
            db.SecretPermissions.Add(new SecretPermission
            {
                UserId = userId,
                SecretId = secretId,
                GrantedBy = grantedBy
            });
        }
        await BumpAsync(db, userId, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task RevokeSecretAsync(long userId, long secretId, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        await db.SecretPermissions
            .Where(sp => sp.UserId == userId && sp.SecretId == secretId)
            .ExecuteDeleteAsync(ct);
        await BumpAsync(db, userId, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task DenySecretAsync(long userId, long secretId, long deniedBy, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        bool exists = await db.SecretDenies
            .AnyAsync(sd => sd.UserId == userId && sd.SecretId == secretId, ct);
        if (!exists)
        {
            db.SecretDenies.Add(new SecretDeny
            {
                UserId = userId,
                SecretId = secretId,
                DeniedBy = deniedBy
            });
        }
        await BumpAsync(db, userId, ct);
        await db.SaveChangesAsync(ct);
    }

    public async Task UndenySecretAsync(long userId, long secretId, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        int removed = await db.SecretDenies
            .Where(sd => sd.UserId == userId && sd.SecretId == secretId)
            .ExecuteDeleteAsync(ct);
        if (removed > 0)
        {
            await BumpAsync(db, userId, ct);
            await db.SaveChangesAsync(ct);
        }
    }

    public async Task<IReadOnlyList<SecretSummary>> ListVisibleSecretsAsync(long userId, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var rows = await db.Secrets.AsNoTracking()
            .Where(s =>
                s.DeletedAt == null && s.Category.DeletedAt == null &&
                (db.CategoryPermissions.Any(cp => cp.UserId == userId && cp.CategoryId == s.CategoryId) ||
                 db.SecretPermissions.Any(sp => sp.UserId == userId && sp.SecretId == s.Id)) &&
                !db.SecretDenies.Any(sd => sd.UserId == userId && sd.SecretId == s.Id))
            .OrderBy(s => s.Category.Name).ThenBy(s => s.Title)
            .Select(s => new SecretSummary(s.Id, s.CategoryId, s.Category.Name, s.Title, s.Notes, s.UpdatedAt))
            .ToListAsync(ct);
        return rows;
    }

    public async Task<bool> CanViewSecretAsync(long userId, long secretId, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.Secrets.AsNoTracking().AnyAsync(s => s.Id == secretId &&
            s.DeletedAt == null && s.Category.DeletedAt == null &&
            (db.CategoryPermissions.Any(cp => cp.UserId == userId && cp.CategoryId == s.CategoryId) ||
             db.SecretPermissions.Any(sp => sp.UserId == userId && sp.SecretId == s.Id)) &&
            !db.SecretDenies.Any(sd => sd.UserId == userId && sd.SecretId == s.Id), ct);
    }

    public async Task<long> GetSyncVersionAsync(long userId, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.PermissionSyncs.AsNoTracking()
            .Where(p => p.UserId == userId)
            .Select(p => (long?)p.Version)
            .FirstOrDefaultAsync(ct) ?? 0L;
    }

    public async Task BumpSyncVersionAsync(long userId, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        await BumpAsync(db, userId, ct);
        await db.SaveChangesAsync(ct);
    }

    private static async Task BumpAsync(PwdManagerContext db, long userId, CancellationToken ct)
    {
        var row = await db.PermissionSyncs.FirstOrDefaultAsync(p => p.UserId == userId, ct);
        if (row is null)
            db.PermissionSyncs.Add(new PermissionSync { UserId = userId, Version = 1 });
        else
            row.Version += 1;
    }
}
