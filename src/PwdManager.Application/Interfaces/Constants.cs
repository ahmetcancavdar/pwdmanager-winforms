namespace PwdManager.Application.Interfaces;

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
    public const string SecretRestore    = "SECRET_RESTORE";
    public const string SecretPurge      = "SECRET_PURGE";
    public const string CategoryAdd      = "CATEGORY_ADD";
    public const string CategoryEdit     = "CATEGORY_EDIT";
    public const string CategoryDelete   = "CATEGORY_DELETE";
    public const string CategoryRestore  = "CATEGORY_RESTORE";
    public const string CategoryPurge    = "CATEGORY_PURGE";
    public const string UserAdd          = "USER_ADD";
    public const string UserUpdate       = "USER_UPDATE";
    public const string UserReset        = "USER_RESET";
    public const string PermissionGrant  = "PERMISSION_GRANT";
    public const string PermissionRevoke = "PERMISSION_REVOKE";
}

public static class AppMetaKeys
{
    public const string SchemaVersion      = "schema_version";
    public const string RecoverySalt       = "recovery_salt";
    public const string RecoveryWrappedDek = "recovery_wrapped_dek";
}
