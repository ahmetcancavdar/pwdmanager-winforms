using PwdManager.Application.Interfaces;
using PwdManager.Application.Security;

namespace PwdManager.Application.Services;

/// <summary>
/// Admin-side grant/revoke/deny. Every call goes straight to the database and bumps the
/// target user's sync counter, so a logged-in personnel sees the change within their
/// poll interval (and immediately on their next reveal attempt).
/// </summary>
public sealed class PermissionService
{
    private readonly IPermissionRepository _permissions;
    private readonly ISecretRepository _secrets;
    private readonly IAuditRepository _audit;

    public PermissionService(IPermissionRepository permissions, ISecretRepository secrets, IAuditRepository audit)
    {
        _permissions = permissions;
        _secrets = secrets;
        _audit = audit;
    }

    public sealed record GrantState(
        IReadOnlySet<long> CategoryIds,
        IReadOnlySet<long> SecretIds,
        IReadOnlySet<long> DeniedSecretIds);

    public async Task<GrantState> GetStateAsync(long userId, CancellationToken ct = default)
    {
        var cats = await _permissions.GetGrantedCategoryIdsAsync(userId, ct);
        var secs = await _permissions.GetGrantedSecretIdsAsync(userId, ct);
        var denied = await _permissions.GetDeniedSecretIdsAsync(userId, ct);
        return new GrantState(cats.ToHashSet(), secs.ToHashSet(), denied.ToHashSet());
    }

    public async Task SetCategoryAsync(SessionContext admin, long userId, long categoryId, bool granted, CancellationToken ct = default)
    {
        admin.EnsureAdmin();
        if (granted)
            await _permissions.GrantCategoryAsync(userId, categoryId, admin.User.Id, ct);
        else
            await _permissions.RevokeCategoryAsync(userId, categoryId, ct);

        await _audit.WriteAsync(
            granted ? AuditAction.PermissionGrant : AuditAction.PermissionRevoke,
            admin.User.Id, admin.User.Username, "category", categoryId, $"user={userId}", ct);
    }

    /// <summary>
    /// Toggle whether a personnel can see one secret.
    /// If the secret's whole category is granted, "not allowed" becomes a carve-out
    /// (deny) instead of a plain revoke — so the exception survives inside the grant.
    /// </summary>
    public async Task SetSecretAsync(SessionContext admin, long userId, long secretId, bool allowed, CancellationToken ct = default)
    {
        admin.EnsureAdmin();

        long categoryId = await _secrets.GetCategoryIdAsync(secretId, ct);
        bool categoryGranted = (await _permissions.GetGrantedCategoryIdsAsync(userId, ct)).Contains(categoryId);

        if (categoryGranted)
        {
            if (allowed)
                await _permissions.UndenySecretAsync(userId, secretId, ct);
            else
                await _permissions.DenySecretAsync(userId, secretId, admin.User.Id, ct);
        }
        else
        {
            if (allowed)
                await _permissions.GrantSecretAsync(userId, secretId, admin.User.Id, ct);
            else
                await _permissions.RevokeSecretAsync(userId, secretId, ct);
            await _permissions.UndenySecretAsync(userId, secretId, ct); // clear any stale carve-out
        }

        await _audit.WriteAsync(
            allowed ? AuditAction.PermissionGrant : AuditAction.PermissionRevoke,
            admin.User.Id, admin.User.Username, "secret", secretId,
            categoryGranted ? $"user={userId};carveout" : $"user={userId}", ct);
    }
}
