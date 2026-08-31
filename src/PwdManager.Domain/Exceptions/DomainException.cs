namespace PwdManager.Domain.Exceptions;

/// <summary>
/// Base type for business-rule violations surfaced to the user (invalid input,
/// duplicate name, forbidden operation, …). Derives from
/// <see cref="InvalidOperationException"/> so existing <c>catch</c> sites keep working.
/// </summary>
public class DomainException : InvalidOperationException
{
    public DomainException(string message) : base(message) { }
    public DomainException(string message, Exception inner) : base(message, inner) { }
}
