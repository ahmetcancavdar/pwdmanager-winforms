using PwdManager.Domain.Enums;

namespace PwdManager.Application.Security;

/// <summary>
/// Minimal identity carried for the lifetime of a session. Deliberately does NOT
/// hold the password hash or wrapped DEK — those are only needed during login.
/// </summary>
public sealed record AuthenticatedUser(long Id, string Username, string FullName, UserRole Role);
