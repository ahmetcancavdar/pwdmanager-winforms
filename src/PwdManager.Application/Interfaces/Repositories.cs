using PwdManager.Application.Models;

namespace PwdManager.Application.Interfaces;

public interface IUserRepository
{
    Task<UserRecord?> FindByUsernameAsync(string username, CancellationToken ct = default);
    Task<UserRecord?> GetByIdAsync(long id, CancellationToken ct = default);

    /// <summary>Cheap check used by the live poll: does this account still exist and is it active?</summary>
    Task<bool> IsActiveAsync(long id, CancellationToken ct = default);
    Task<IReadOnlyList<UserRecord>> ListAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<UserRecord>> ListPersonnelAsync(CancellationToken ct = default);

    Task<long> CreateAsync(NewUser user, CancellationToken ct = default);
    Task UpdateProfileAsync(long id, string fullName, bool isActive, CancellationToken ct = default);

    /// <summary>Replace credentials: password hash, KEK salt, freshly wrapped DEK.</summary>
    Task SetCredentialsAsync(long id, string passwordHash, byte[] kdfSalt, byte[] wrappedDek,
        bool mustChangePassword, CancellationToken ct = default);

    Task RegisterFailedLoginAsync(long id, int failedCount, DateTime? lockedUntil, CancellationToken ct = default);
    Task ClearLoginFailuresAsync(long id, CancellationToken ct = default);
}

public interface ICategoryRepository
{
    Task<IReadOnlyList<CategoryRecord>> ListAsync(CancellationToken ct = default);
    Task<IReadOnlyList<CategoryRecord>> ListDeletedAsync(CancellationToken ct = default);
    Task<CategoryRecord?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<CategoryRecord?> GetByIdRawAsync(long id, CancellationToken ct = default);

    Task<long> CreateAsync(string name, string description, long createdBy, CancellationToken ct = default);
    Task UpdateAsync(long id, string name, string description, CancellationToken ct = default);

    Task SoftDeleteAsync(long id, long deletedBy, CancellationToken ct = default);
    Task RestoreAsync(long id, CancellationToken ct = default);
    Task PurgeAsync(long id, CancellationToken ct = default);

    Task<int> CountActiveSecretsAsync(long categoryId, CancellationToken ct = default);
    Task<bool> ActiveNameExistsAsync(string name, long? excludingId = null, CancellationToken ct = default);
}

public interface ISecretRepository
{
    Task<IReadOnlyList<SecretSummary>> ListAllSummariesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<SecretSummary>> ListSummariesByCategoryAsync(long categoryId, CancellationToken ct = default);
    Task<IReadOnlyList<SecretSummary>> ListDeletedAsync(CancellationToken ct = default);

    Task<SecretRecord?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<SecretRecord?> GetByIdRawAsync(long id, CancellationToken ct = default);
    Task<long> GetCategoryIdAsync(long secretId, CancellationToken ct = default);

    Task<long> CreateAsync(NewSecret secret, CancellationToken ct = default);
    Task UpdateAsync(long id, long categoryId, string title, byte[]? usernameCipher, byte[] secretCipher,
        string notes, CancellationToken ct = default);

    Task SoftDeleteAsync(long id, long deletedBy, CancellationToken ct = default);
    Task RestoreAsync(long id, CancellationToken ct = default);
    Task PurgeAsync(long id, CancellationToken ct = default);
}

/// <summary>
/// Access grants and the per-user sync counter. Effective visibility of a secret =
/// (category granted OR secret granted) AND NOT denied AND not soft-deleted.
/// </summary>
public interface IPermissionRepository
{
    Task<IReadOnlyList<long>> GetGrantedCategoryIdsAsync(long userId, CancellationToken ct = default);
    Task<IReadOnlyList<long>> GetGrantedSecretIdsAsync(long userId, CancellationToken ct = default);
    Task<IReadOnlyList<long>> GetDeniedSecretIdsAsync(long userId, CancellationToken ct = default);

    Task GrantCategoryAsync(long userId, long categoryId, long grantedBy, CancellationToken ct = default);
    Task RevokeCategoryAsync(long userId, long categoryId, CancellationToken ct = default);
    Task GrantSecretAsync(long userId, long secretId, long grantedBy, CancellationToken ct = default);
    Task RevokeSecretAsync(long userId, long secretId, CancellationToken ct = default);

    Task DenySecretAsync(long userId, long secretId, long deniedBy, CancellationToken ct = default);
    Task UndenySecretAsync(long userId, long secretId, CancellationToken ct = default);

    Task<IReadOnlyList<SecretSummary>> ListVisibleSecretsAsync(long userId, CancellationToken ct = default);
    Task<bool> CanViewSecretAsync(long userId, long secretId, CancellationToken ct = default);

    Task<long> GetSyncVersionAsync(long userId, CancellationToken ct = default);
    Task BumpSyncVersionAsync(long userId, CancellationToken ct = default);
}

/// <summary>
/// Per (user, secret) reveal re-authentication attempt tracking — independent of the
/// account-wide login lockout. Survives UI refresh/restart because it lives in the DB.
/// </summary>
public interface IRevealLockRepository
{
    /// <summary>Current failure count and lock deadline (if any) for this user+secret.</summary>
    Task<(int FailedCount, DateTime? LockedUntil)> GetAsync(long userId, long secretId, CancellationToken ct = default);

    /// <summary>
    /// Records one more failed re-auth attempt. If an existing lock has already
    /// expired the counter starts a fresh window. Once <paramref name="maxAttempts"/>
    /// is reached, locks for <paramref name="lockoutMinutes"/> from now.
    /// </summary>
    Task<(int FailedCount, DateTime? LockedUntil)> RegisterFailureAsync(
        long userId, long secretId, int maxAttempts, int lockoutMinutes, CancellationToken ct = default);

    /// <summary>Clears the counter/lock — called after a successful re-auth.</summary>
    Task ClearAsync(long userId, long secretId, CancellationToken ct = default);
}

public interface IAuditRepository
{
    Task WriteAsync(string action, long? userId, string username,
        string targetType = "", long? targetId = null, string detail = "", CancellationToken ct = default);

    Task<IReadOnlyList<AuditRecord>> RecentAsync(int limit, CancellationToken ct = default);
}

/// <summary>Small binary key/value store (schema version, recovery salt, recovery-wrapped DEK).</summary>
public interface IAppMetaRepository
{
    Task<byte[]?> GetAsync(string key, CancellationToken ct = default);
    Task SetAsync(string key, byte[] value, CancellationToken ct = default);
}
