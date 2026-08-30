using System.Security.Cryptography;

namespace PwdManager.Core.Cryptography;

/// <summary>
/// Argon2id password hashing for login verification. Produces a self-describing
/// encoded string: <c>$argon2id$v=19$m=&lt;kib&gt;,t=&lt;iters&gt;,p=&lt;par&gt;$&lt;b64salt&gt;$&lt;b64hash&gt;</c>
/// </summary>
public static class PasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;

    public static string Hash(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Argon2Kdf.Derive(password, salt,
            Argon2Kdf.DefaultMemoryKib, Argon2Kdf.DefaultIterations, Argon2Kdf.DefaultParallelism, HashSize);

        return $"$argon2id$v=19$m={Argon2Kdf.DefaultMemoryKib},t={Argon2Kdf.DefaultIterations},p={Argon2Kdf.DefaultParallelism}$" +
               $"{Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    public static bool Verify(string password, string encoded)
    {
        try
        {
            // ["argon2id", "v=19", "m=..,t=..,p=..", "<b64salt>", "<b64hash>"]
            string[] parts = encoded.Split('$', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 5 || parts[0] != "argon2id")
                return false;

            string[] p = parts[2].Split(',');
            int memoryKib = int.Parse(p[0].AsSpan(2));
            int iterations = int.Parse(p[1].AsSpan(2));
            int parallelism = int.Parse(p[2].AsSpan(2));

            byte[] salt = Convert.FromBase64String(parts[3]);
            byte[] expected = Convert.FromBase64String(parts[4]);
            byte[] actual = Argon2Kdf.Derive(password, salt, memoryKib, iterations, parallelism, expected.Length);

            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }
        catch
        {
            return false;
        }
    }
}
