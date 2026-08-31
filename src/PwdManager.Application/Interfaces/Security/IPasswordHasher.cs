namespace PwdManager.Application.Interfaces;

/// <summary>
/// Login-password hashing. The concrete algorithm (Argon2id) lives in Infrastructure;
/// Application only sees "hash" and "verify".
/// </summary>
public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string encoded);
}
