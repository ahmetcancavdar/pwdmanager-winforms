using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using PwdManager.Application.Configuration;
using PwdManager.Application.Interfaces;

namespace PwdManager.Infrastructure.Configuration;

/// <summary>
/// Loads <see cref="AppConfig"/> from <c>appsettings.json</c> (template, committed) plus
/// <c>appsettings.local.json</c> (machine-local, git-ignored). The DB password in the local
/// file is DPAPI-protected (CurrentUser scope) so it is unreadable by other Windows accounts
/// and never stored in clear text.
/// </summary>
public sealed class ConfigStore : IConfigStore
{
    private const string LocalFileName = "appsettings.local.json";

    // Extra entropy mixed into DPAPI — not a secret, just app-specific domain separation.
    private static readonly byte[] Entropy = Encoding.UTF8.GetBytes("PwdManager.ConnectionSecret.v1");

    private static string BaseDirectory => AppContext.BaseDirectory;
    private static string LocalFilePath => Path.Combine(BaseDirectory, LocalFileName);

    public AppConfig Load()
    {
        IConfigurationRoot root = new ConfigurationBuilder()
            .SetBasePath(BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile(LocalFileName, optional: true)
            .Build();

        var config = new AppConfig();
        root.Bind(config);

        if (!string.IsNullOrEmpty(config.Database.ProtectedPassword))
            config.Database.Password = Unprotect(config.Database.ProtectedPassword);

        return config;
    }

    public void SaveDatabase(DatabaseConfig db)
    {
        var payload = new
        {
            Database = new
            {
                db.Host,
                db.Port,
                db.Name,
                db.User,
                ProtectedPassword = string.IsNullOrEmpty(db.Password) ? "" : Protect(db.Password)
            }
        };

        string json = JsonSerializer.Serialize(payload, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(LocalFilePath, json);
    }

    public bool LocalConfigExists() => File.Exists(LocalFilePath);

    private static string Protect(string plaintext)
    {
        byte[] blob = ProtectedData.Protect(
            Encoding.UTF8.GetBytes(plaintext), Entropy, DataProtectionScope.CurrentUser);
        return Convert.ToBase64String(blob);
    }

    private static string Unprotect(string protectedBase64)
    {
        try
        {
            byte[] blob = ProtectedData.Unprotect(
                Convert.FromBase64String(protectedBase64), Entropy, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(blob);
        }
        catch (CryptographicException)
        {
            return "";
        }
    }
}
