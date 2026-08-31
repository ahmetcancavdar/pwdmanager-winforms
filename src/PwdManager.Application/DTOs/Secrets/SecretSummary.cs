namespace PwdManager.Application.DTOs;

/// <summary>Lightweight row for list/tree views. No cipher material — safe to hold in UI state.</summary>
public sealed record SecretSummary(
    long Id, long CategoryId, string CategoryName, string Title, string Notes, DateTime UpdatedAt);
