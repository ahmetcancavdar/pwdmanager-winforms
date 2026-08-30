using System.Text;
using Konscious.Security.Cryptography;

namespace PwdManager.Core.Cryptography;

/// <summary>
/// Thin wrapper over Argon2id. Shared by <see cref="PasswordHasher"/> (login
/// verification) and <see cref="KeyDerivation"/> (deriving the KEK).
/// </summary>
internal static class Argon2Kdf
{
    public const int DefaultMemoryKib = 65536; // 64 MiB
    public const int DefaultIterations = 3;
    public const int DefaultParallelism = 2;

    public static byte[] Derive(
        string password,
        byte[] salt,
        int memoryKib,
        int iterations,
        int parallelism,
        int outputBytes)
    {
        using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = salt,
            MemorySize = memoryKib,
            Iterations = iterations,
            DegreeOfParallelism = parallelism
        };
        return argon2.GetBytes(outputBytes);
    }
}
