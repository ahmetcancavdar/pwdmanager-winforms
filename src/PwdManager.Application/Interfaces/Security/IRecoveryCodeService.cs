namespace PwdManager.Application.Interfaces;

/// <summary>Human-transcribable recovery code generation + normalisation.</summary>
public interface IRecoveryCodeService
{
    string Generate();
    string Normalize(string input);
}
