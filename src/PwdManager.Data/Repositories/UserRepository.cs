using Microsoft.EntityFrameworkCore;
using PwdManager.Data.Entities;
using PwdManager.Data.Persistence;

namespace PwdManager.Data.Repositories;

public sealed class UserRepository : IUserRepository
{
    private readonly IDbContextFactory<PwdManagerContext> _factory;

    public UserRepository(IDbContextFactory<PwdManagerContext> factory) => _factory = factory;

    public async Task<User?> FindByUsernameAsync(string username, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == username, ct);
    }

    public async Task<User?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);
    }

    public async Task<bool> IsActiveAsync(long id, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.Users.AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => u.IsActive)
            .FirstOrDefaultAsync(ct) == true;
    }

    public async Task<IReadOnlyList<User>> ListAllAsync(CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.Users.AsNoTracking()
            .OrderBy(u => u.Role).ThenBy(u => u.Username).ToListAsync(ct);
    }

    public async Task<IReadOnlyList<User>> ListPersonnelAsync(CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        return await db.Users.AsNoTracking()
            .Where(u => u.Role == "Personnel")
            .OrderBy(u => u.Username).ToListAsync(ct);
    }

    public async Task<long> CreateAsync(User user, CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);
        db.Users.Add(user);
        await db.SaveChangesAsync(ct);
        return user.Id;
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
