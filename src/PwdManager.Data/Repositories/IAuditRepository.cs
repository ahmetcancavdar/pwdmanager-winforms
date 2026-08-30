using PwdManager.Data.Entities;

namespace PwdManager.Data.Repositories;

public static class AuditAction
{
    public const string Login            = "LOGIN";
    public const string LoginFailed      = "LOGIN_FAILED";
    public const string Logout           = "LOGOUT";
    public const string PasswordChange   = "PASSWORD_CHANGE";
    public const string SecretView       = "SECRET_VIEW";
    public const string SecretViewDenied = "SECRET_VIEW_DENIED";
    public const string RevealAuthFailed = "REVEAL_AUTH_FAILED";
    public const string SecretAdd        = "SECRET_ADD";
    public const string SecretEdit       = "SECRET_EDIT";
    public const string SecretDelete     = "SECRET_DELETE";
    public const string CategoryAdd      = "CATEGORY_ADD";
    public const string CategoryEdit     = "CATEGORY_EDIT";
    public const string CategoryDelete   = "CATEGORY_DELETE";
    public const string UserAdd          = "USER_ADD";
    public const string UserUpdate       = "USER_UPDATE";
    public const string UserReset        = "USER_RESET";
    public const string PermissionGrant  = "PERMISSION_GRANT";
    public const string PermissionRevoke = "PERMISSION_REVOKE";
}

public interface IAuditRepository
{
    Task WriteAsync(string action, long? userId, string username,
        string targetType = "", long? targetId = null, string detail = "", CancellationToken ct = default);

    Task<IReadOnlyList<AuditLog>> RecentAsync(int limit, CancellationToken ct = default);
}
