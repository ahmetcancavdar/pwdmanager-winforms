namespace PwdManager.Application.Interfaces;

/// <summary>Derives a 256-bit key-encryption key (KEK) from a password + salt.</summary>
public interface IKeyDerivation
{
    byte[] NewSalt();
    byte[] DeriveKey(string password, byte[] salt);
}
