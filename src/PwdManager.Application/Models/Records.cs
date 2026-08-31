using PwdManager.Domain.Enums;

namespace PwdManager.Application.Models;

/// <summary>Lightweight row for list/tree views. No cipher material — safe to hold in UI state.</summary>
public sealed record SecretSummary(
    long Id, long CategoryId, string CategoryName, string Title, string Notes, DateTime UpdatedAt);

/// <summary>A user account as the application sees it (repository → service).</summary>
public sealed record UserRecord(
    long Id, string Username, string FullName, UserRole Role,
    string PasswordHash, byte[] KdfSalt, byte[] WrappedDek,
    bool IsActive, bool MustChangePassword, int FailedLoginCount, DateTime? LockedUntil, DateTime CreatedAt);

/// <summary>Input for creating a user (service → repository).</summary>
public sealed record NewUser(
    string Username, string FullName, UserRole Role,
    string PasswordHash, byte[] KdfSalt, byte[] WrappedDek,
    bool IsActive, bool MustChangePassword);

public sealed record CategoryRecord(
    long Id, string Name, string Description, long? CreatedBy, DateTime CreatedAt, DateTime? DeletedAt, long? DeletedBy);

/// <summary>A stored credential incl. cipher blobs — only for admin edit or a permitted reveal.</summary>
public sealed record SecretRecord(
    long Id, long CategoryId, string Title, byte[]? UsernameCipher, byte[] SecretCipher, string Notes,
    DateTime CreatedAt, DateTime UpdatedAt, DateTime? DeletedAt);

public sealed record NewSecret(
    long CategoryId, string Title, byte[]? UsernameCipher, byte[] SecretCipher, string Notes, long? CreatedBy);

public sealed record AuditRecord(
    DateTime CreatedAt, string Username, string Action, string TargetType, long? TargetId, string Detail);
