using PwdManager.Domain.Enums;

namespace PwdManager.Application.DTOs;

/// <summary>Input for creating a user account (service → repository).</summary>
public sealed record NewUser(
    string Username, string FullName, UserRole Role,
    string PasswordHash, byte[] KdfSalt, byte[] WrappedDek,
    bool IsActive, bool MustChangePassword);
