namespace PwdManager.Application.DTOs;

/// <summary>Input for creating a stored credential (service → repository); ciphers already sealed.</summary>
public sealed record NewSecret(
    long CategoryId, string Title, byte[]? UsernameCipher, byte[] SecretCipher, string Notes, long? CreatedBy);
