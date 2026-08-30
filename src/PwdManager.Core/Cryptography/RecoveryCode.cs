using System.Security.Cryptography;
using System.Text;

namespace PwdManager.Core.Cryptography;

/// <summary>
/// A human-transcribable recovery code (e.g. <c>H7K2P-9RMTX-...</c>). Generated once
/// during setup, shown to the admin, and never stored. It derives a KEK that wraps
/// a recovery copy of the DEK so the system survives loss of all admin passwords.
/// </summary>
public static class RecoveryCode
{
    // Crockford-style alphabet: no I, L, O, U, 0, 1 to avoid transcription errors.
    private const string Alphabet = "ABCDEFGHJKMNPQRSTVWXYZ23456789";

    public static string Generate(int groups = 6, int groupLength = 5)
    {
        int total = groups * groupLength;
        Span<byte> raw = stackalloc byte[total];
        RandomNumberGenerator.Fill(raw);

        var sb = new StringBuilder(total + groups);
        for (int i = 0; i < total; i++)
        {
            if (i > 0 && i % groupLength == 0) sb.Append('-');
            sb.Append(Alphabet[raw[i] % Alphabet.Length]);
        }
        return sb.ToString();
    }

    /// <summary>Strips spaces/dashes and upper-cases, so user formatting doesn't matter.</summary>
    public static string Normalize(string input)
    {
        var sb = new StringBuilder(input.Length);
        foreach (char c in input)
            if (char.IsLetterOrDigit(c))
                sb.Append(char.ToUpperInvariant(c));
        return sb.ToString();
    }
}
