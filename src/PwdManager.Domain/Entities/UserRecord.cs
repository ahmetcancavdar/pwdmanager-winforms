using PwdManager.Domain.Enums;

namespace PwdManager.Domain.Entities;

/// <summary>
/// A user account as the domain sees it — the shape repositories return and
/// services reason about. Immutable; carries no EF/persistence concern.
/// </summary>
public sealed record UserRecord(
    long Id, string Username, string FullName, UserRole Role,
    string PasswordHash, byte[] KdfSalt, byte[] WrappedDek,
    bool IsActive, bool MustChangePassword, int FailedLoginCount, DateTime? LockedUntil, DateTime CreatedAt);
