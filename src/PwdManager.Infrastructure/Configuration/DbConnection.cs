using MySqlConnector;
using PwdManager.Application.Configuration;

namespace PwdManager.Infrastructure.Configuration;

/// <summary>Builds a MySQL/MariaDB connection string from <see cref="DatabaseConfig"/>.</summary>
internal static class DbConnection
{
    public static string BuildConnectionString(DatabaseConfig db, bool includeDatabase = true)
    {
        var b = new MySqlConnectionStringBuilder
        {
            Server = db.Host,
            Port = (uint)db.Port,
            UserID = db.User,
            Password = db.Password,
            SslMode = MySqlSslMode.Preferred,
            ConnectionTimeout = 10u,
            DefaultCommandTimeout = 30u,
            Pooling = true
        };
        if (includeDatabase)
            b.Database = db.Name;
        return b.ConnectionString;
    }
}
