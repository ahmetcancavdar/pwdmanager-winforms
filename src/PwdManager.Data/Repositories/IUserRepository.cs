using PwdManager.Data.Entities;

namespace PwdManager.Data.Repositories;

public interface IUserRepository
{
    Task<User?> FindByUsernameAsync(string username, CancellationToken ct = default);
    Task<User?> GetByIdAsync(long id, CancellationToken ct = default);

    /// <summary>Cheap check used by the live poll: does this account still exist and is it active?</summary>
    Task<bool> IsActiveAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<User>> ListAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<User>> ListPersonnelAsync(CancellationToken ct = default);

    Task<long> CreateAsync(User user, CancellationToken ct = default);
    Task UpdateProfileAsync(long id, string fullName, bool isActive, CancellationToken ct = default);

    /// <summary>Replace credentials: password hash, KEK salt, freshly wrapped DEK.</summary>
    Task SetCredentialsAsync(long id, string passwordHash, byte[] kdfSalt, byte[] wrappedDek,
        bool mustChangePassword, CancellationToken ct = default);

    Task RegisterFailedLoginAsync(long id, int failedCount, DateTime? lockedUntil, CancellationToken ct = default);
    Task ClearLoginFailuresAsync(long id, CancellationToken ct = default);
}
