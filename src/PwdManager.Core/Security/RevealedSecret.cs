namespace PwdManager.Core.Security;

/// <summary>Decrypted credential handed to the UI for a few seconds, then cleared.</summary>
public sealed class RevealedSecret
{
    public string Title { get; set; } = "";
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Notes { get; set; } = "";
}
