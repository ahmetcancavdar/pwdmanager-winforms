using PwdManager.Application.Security;

namespace PwdManager.Application.DTOs;

/// <summary>
/// Result of a login attempt: a status the UI can branch on, plus the live
/// <see cref="SessionContext"/> on success and a lockout deadline when relevant.
/// </summary>
public sealed record LoginOutcome(LoginStatus Status, SessionContext? Session, DateTime? LockedUntil);
