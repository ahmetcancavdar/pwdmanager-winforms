using MySqlConnector;

namespace PwdManager.Data.Infrastructure;

/// <summary>MySQL connection settings, loaded from appsettings(.local).json.</summary>
public sealed class DbOptions
{
    public string Host { get; set; } = "127.0.0.1";
    public int Port { get; set; } = 3306;
    public string Name { get; set; } = "pwdmanager";
    public string User { get; set; } = "";
    public string Password { get; set; } = "";

    public string BuildConnectionString(bool includeDatabase = true)
    {
        var b = new MySqlConnectionStringBuilder
        {
            Server = Host,
            Port = (uint)Port,
            UserID = User,
            Password = Password,
            SslMode = MySqlSslMode.Preferred,
            ConnectionTimeout = 10u,
            DefaultCommandTimeout = 30u,
            Pooling = true
        };
        if (includeDatabase)
            b.Database = Name;
        return b.ConnectionString;
    }
}
