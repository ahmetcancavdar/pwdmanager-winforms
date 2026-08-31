namespace PwdManager.Domain.Entities;

/// <summary>A category as the domain sees it. <see cref="DeletedAt"/> non-null ⇒ soft-deleted (in "Silinenler").</summary>
public sealed record CategoryRecord(
    long Id, string Name, string Description, long? CreatedBy, DateTime CreatedAt, DateTime? DeletedAt, long? DeletedBy);
