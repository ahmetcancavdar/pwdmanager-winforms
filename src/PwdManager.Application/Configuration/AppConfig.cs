namespace PwdManager.Application.Configuration;

/// <summary>Strongly-typed application settings (from appsettings.json + appsettings.local.json).</summary>
public sealed class AppConfig
{
    public DatabaseConfig Database { get; set; } = new();
    public SecurityConfig Security { get; set; } = new();
}

public sealed class DatabaseConfig
{
    public string Host { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 3306;
    public string Name { get; set; } = "pwdmanager";
    public string User { get; set; } = "";

    /// <summary>Plaintext password (template only; normally empty).</summary>
    public string Password { get; set; } = "";

    /// <summary>Base64 DPAPI-protected password, written to appsettings.local.json by the setup wizard.</summary>
    public string ProtectedPassword { get; set; } = "";
}

public sealed class SecurityConfig
{
    public int IdleLockMinutes { get; set; } = 5;
    public int LoginMaxAttempts { get; set; } = 5;

    /// <summary>How long an account stays locked after <see cref="LoginMaxAttempts"/> consecutive failed logins.</summary>
    public int LockoutMinutes { get; set; } = 10;
    public int RevealMaxAttempts { get; set; } = 3;

    /// <summary>How long a single secret stays reveal-locked after <see cref="RevealMaxAttempts"/> wrong re-auth attempts.</summary>
    public int RevealLockoutMinutes { get; set; } = 5;
    public int RevealVisibleSeconds { get; set; } = 20;
    public int PermissionPollSeconds { get; set; } = 2;
}
