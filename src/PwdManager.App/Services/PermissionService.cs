using PwdManager.Core.Security;
using PwdManager.Data.Repositories;

namespace PwdManager.App.Services;

/// <summary>
/// Admin-side grant/revoke. Every call goes straight to the database and bumps the
/// target user's sync counter, so a logged-in personnel sees the change within their
/// poll interval (and immediately on their next reveal attempt).
/// </summary>
public sealed class PermissionService
{
    private readonly IPermissionRepository _permissions;
    private readonly IAuditRepository _audit;

    public PermissionService(IPermissionRepository permissions, IAuditRepository audit)
    {
        _permissions = permissions;
        _audit = audit;
    }

    public sealed record GrantState(IReadOnlySet<long> CategoryIds, IReadOnlySet<long> SecretIds);

    public async Task<GrantState> GetStateAsync(long userId, CancellationToken ct = default)
    {
        var cats = await _permissions.GetGrantedCategoryIdsAsync(userId, ct);
        var secs = await _permissions.GetGrantedSecretIdsAsync(userId, ct);
        return new GrantState(cats.ToHashSet(), secs.ToHashSet());
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

    public async Task SetSecretAsync(SessionContext admin, long userId, long secretId, bool granted, CancellationToken ct = default)
    {
        admin.EnsureAdmin();
        if (granted)
            await _permissions.GrantSecretAsync(userId, secretId, admin.User.Id, ct);
        else
            await _permissions.RevokeSecretAsync(userId, secretId, ct);

        await _audit.WriteAsync(
            granted ? AuditAction.PermissionGrant : AuditAction.PermissionRevoke,
            admin.User.Id, admin.User.Username, "secret", secretId, $"user={userId}", ct);
    }
}
