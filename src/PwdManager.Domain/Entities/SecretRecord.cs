namespace PwdManager.Domain.Entities;

/// <summary>
/// A stored credential incl. its cipher blobs — only materialised for an admin edit
/// or a permitted reveal. <see cref="DeletedAt"/> non-null ⇒ soft-deleted.
/// </summary>
public sealed record SecretRecord(
    long Id, long CategoryId, string Title, byte[]? UsernameCipher, byte[] SecretCipher, string Notes,
    DateTime CreatedAt, DateTime UpdatedAt, DateTime? DeletedAt);
