using Microsoft.EntityFrameworkCore;
using PwdManager.Application.Interfaces;
using PwdManager.Infrastructure.Entities;
using PwdManager.Infrastructure.Persistence;

namespace PwdManager.Infrastructure.Repositories;

public sealed class RevealLockRepository : IRevealLockRepository
{
    private readonly IDbContextFactory<PwdManagerContext> _factory;

    public RevealLockRepository(IDbContextFactory<PwdManagerContext> factory) => _factory = factory;

    public async Task<(int FailedCount, DateTime? LockedUntil)> GetAsync(long userId, long secretId, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var row = await db.SecretRevealLocks.AsNoTracking()
            .FirstOrDefaultAsync(l => l.UserId == userId && l.SecretId == secretId, ct);
        return row is null ? (0, null) : (row.FailedCount, row.LockedUntil);
    }

    public async Task<(int FailedCount, DateTime? LockedUntil)> RegisterFailureAsync(
        long userId, long secretId, int maxAttempts, int lockoutMinutes, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var row = await db.SecretRevealLocks
            .FirstOrDefaultAsync(l => l.UserId == userId && l.SecretId == secretId, ct);

        var now = DateTime.UtcNow;
        if (row is null)
        {
            row = new SecretRevealLock { UserId = userId, SecretId = secretId, FailedCount = 0, LockedUntil = null };
            db.SecretRevealLocks.Add(row);
        }
        else if (row.LockedUntil is { } expired && expired <= now)
        {
            // Previous lock window has passed — start counting fresh.
            row.FailedCount = 0;
            row.LockedUntil = null;
        }

        row.FailedCount++;
        if (row.FailedCount >= maxAttempts)
            row.LockedUntil = now.AddMinutes(lockoutMinutes);

        await db.SaveChangesAsync(ct);
        return (row.FailedCount, row.LockedUntil);
    }

    public async Task ClearAsync(long userId, long secretId, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        await db.SecretRevealLocks
            .Where(l => l.UserId == userId && l.SecretId == secretId)
            .ExecuteDeleteAsync(ct);
    }
}
