using Microsoft.EntityFrameworkCore;
using PwdManager.Data.Entities;
using PwdManager.Data.Persistence;

namespace PwdManager.Data.Repositories;

public sealed class AppMetaRepository : IAppMetaRepository
{
    private readonly IDbContextFactory<PwdManagerContext> _factory;

    public AppMetaRepository(IDbContextFactory<PwdManagerContext> factory) => _factory = factory;

    public async Task<byte[]?> GetAsync(string key, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.AppMeta.AsNoTracking()
            .Where(m => m.MetaKey == key)
            .Select(m => m.MetaValue)
            .FirstOrDefaultAsync(ct);
    }

    public async Task SetAsync(string key, byte[] value, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var row = await db.AppMeta.FirstOrDefaultAsync(m => m.MetaKey == key, ct);
        if (row is null)
            db.AppMeta.Add(new AppMetum { MetaKey = key, MetaValue = value });
        else
            row.MetaValue = value;
        await db.SaveChangesAsync(ct);
    }
}
