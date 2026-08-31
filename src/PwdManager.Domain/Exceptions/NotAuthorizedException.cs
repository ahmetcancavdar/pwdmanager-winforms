namespace PwdManager.Domain.Exceptions;

/// <summary>Thrown when a session attempts an operation its role does not permit.</summary>
public sealed class NotAuthorizedException : DomainException
{
    public NotAuthorizedException(string message) : base(message) { }
}
