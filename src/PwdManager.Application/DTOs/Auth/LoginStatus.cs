namespace PwdManager.Application.DTOs;

/// <summary>The only login result the UI ever sees — never AES/Argon2/EF/MySQL detail.</summary>
public enum LoginStatus
{
    Success,
    InvalidCredentials,
    Inactive,
    LockedOut
}
