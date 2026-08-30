namespace PwdManager.Data.Repositories;

/// <summary>
/// Lightweight row for list/tree views. Carries no cipher material, so it is safe
/// to hold in UI state and pass around freely.
/// </summary>
public sealed record SecretSummary(
    long Id,
    long CategoryId,
    string CategoryName,
    string Title,
    string Notes,
    DateTime UpdatedAt);
