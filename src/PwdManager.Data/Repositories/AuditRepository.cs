using Microsoft.EntityFrameworkCore;
using PwdManager.Data.Entities;
using PwdManager.Data.Persistence;

namespace PwdManager.Data.Repositories;

public sealed class AuditRepository : IAuditRepository
{
    private readonly IDbContextFactory<PwdManagerContext> _factory;

    public AuditRepository(IDbContextFactory<PwdManagerContext> factory) => _factory = factory;

    public async Task WriteAsync(string action, long? userId, string username,
        string targetType = "", long? targetId = null, string detail = "", CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        db.AuditLogs.Add(new AuditLog
        {
            Action = action,
            UserId = userId,
            Username = username,
            TargetType = targetType,
            TargetId = targetId,
            Detail = detail.Length > 255 ? detail[..255] : detail
        });
        await db.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<AuditLog>> RecentAsync(int limit, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.AuditLogs.AsNoTracking()
            .OrderByDescending(a => a.Id).Take(limit).ToListAsync(ct);
    }
}
