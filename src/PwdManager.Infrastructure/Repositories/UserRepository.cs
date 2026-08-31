using Microsoft.EntityFrameworkCore;
using PwdManager.Application.Interfaces;
using PwdManager.Application.Models;
using PwdManager.Infrastructure.Entities;
using PwdManager.Infrastructure.Persistence;

namespace PwdManager.Infrastructure.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly IDbContextFactory<PwdManagerContext> _factory;

    public UserRepository(IDbContextFactory<PwdManagerContext> factory) => _factory = factory;

    public async Task<UserRecord?> FindByUsernameAsync(string username, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var e = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username, ct);
        return e?.ToRecord();
    }

    public async Task<UserRecord?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var e = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);
        return e?.ToRecord();
    }

    public async Task<bool> IsActiveAsync(long id, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.Users.AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => u.IsActive)
            .FirstOrDefaultAsync(ct) == true;
    }

    public async Task<IReadOnlyList<UserRecord>> ListAllAsync(CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var rows = await db.Users.AsNoTracking().OrderBy(u => u.Role).ThenBy(u => u.Username).ToListAsync(ct);
        return rows.Select(Map.ToRecord).ToList();
    }

    public async Task<IReadOnlyList<UserRecord>> ListPersonnelAsync(CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var rows = await db.Users.AsNoTracking()
            .Where(u => u.Role == "Personnel").OrderBy(u => u.Username).ToListAsync(ct);
        return rows.Select(Map.ToRecord).ToList();
    }

    public async Task<long> CreateAsync(NewUser user, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var e = new User
        {
            Username = user.Username,
            FullName = user.FullName,
            Role = user.Role.ToString(),
            PasswordHash = user.PasswordHash,
            KdfSalt = user.KdfSalt,
            WrappedDek = user.WrappedDek,
            IsActive = user.IsActive,
            MustChangePw = user.MustChangePassword
        };
        db.Users.Add(e);
        await db.SaveChangesAsync(ct);
        return e.Id;
    }

    public async Task UpdateProfileAsync(long id, string fullName, bool isActive, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id, ct)
                   ?? throw new InvalidOperationException($"User {id} not found.");
        user.FullName = fullName;
        user.IsActive = isActive;
        await db.SaveChangesAsync(ct);
    }

    public async Task SetCredentialsAsync(long id, string passwordHash, byte[] kdfSalt, byte[] wrappedDek,
        bool mustChangePassword, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id, ct)
                   ?? throw new InvalidOperationException($"User {id} not found.");
        user.PasswordHash = passwordHash;
        user.KdfSalt = kdfSalt;
        user.WrappedDek = wrappedDek;
        user.MustChangePw = mustChangePassword;
        user.FailedLoginCount = 0;
        user.LockedUntil = null;
        await db.SaveChangesAsync(ct);
    }

    public async Task RegisterFailedLoginAsync(long id, int failedCount, DateTime? lockedUntil, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
        if (user is null) return;
        user.FailedLoginCount = failedCount;
        user.LockedUntil = lockedUntil;
        await db.SaveChangesAsync(ct);
    }

    public async Task ClearLoginFailuresAsync(long id, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
        if (user is null) return;
        user.FailedLoginCount = 0;
        user.LockedUntil = null;
        await db.SaveChangesAsync(ct);
    }
}
