using System.Reflection;
using MySqlConnector;

namespace PwdManager.Data.Infrastructure;

/// <summary>
/// First-run helpers used by the setup wizard: test the server, apply the schema,
/// and report whether the app has been initialised yet. Uses raw ADO.NET so it
/// works before the EF <c>DbContext</c> (and its database) exist.
/// </summary>
public sealed class DatabaseInstaller
{
    private readonly DbOptions _options;

    public DatabaseInstaller(DbOptions options) => _options = options;

    /// <summary>Connect to the server without selecting a database.</summary>
    public async Task TestServerConnectionAsync(CancellationToken ct = default)
    {
        await using var c = new MySqlConnection(_options.BuildConnectionString(includeDatabase: false));
        await c.OpenAsync(ct);
    }

    public async Task<bool> SchemaExistsAsync(CancellationToken ct = default)
    {
        await using var c = new MySqlConnection(_options.BuildConnectionString(includeDatabase: false));
        await c.OpenAsync(ct);
        await using var cmd = new MySqlCommand(
            @"SELECT COUNT(*) FROM information_schema.tables
              WHERE table_schema = @db AND table_name = 'users'", c);
        cmd.Parameters.AddWithValue("@db", _options.Name);
        return Convert.ToInt64(await cmd.ExecuteScalarAsync(ct)) > 0;
    }

    public async Task<bool> HasAdminAsync(CancellationToken ct = default)
    {
        await using var c = new MySqlConnection(_options.BuildConnectionString(includeDatabase: true));
        await c.OpenAsync(ct);
        await using var cmd = new MySqlCommand("SELECT COUNT(*) FROM users WHERE role = 'Admin'", c);
        return Convert.ToInt64(await cmd.ExecuteScalarAsync(ct)) > 0;
    }

    /// <summary>Create the database (if missing) and every table. Idempotent.</summary>
    public async Task ApplySchemaAsync(CancellationToken ct = default)
    {
        string script = LoadSchemaScript();
        await using var c = new MySqlConnection(_options.BuildConnectionString(includeDatabase: false));
        await c.OpenAsync(ct);
        // MySqlConnector runs multiple ';'-separated statements in a single command.
        await using var cmd = new MySqlCommand(script, c);
        await cmd.ExecuteNonQueryAsync(ct);
    }

    private static string LoadSchemaScript()
    {
        Assembly asm = typeof(DatabaseInstaller).Assembly;
        string resource = asm.GetManifestResourceNames()
            .Single(n => n.EndsWith("schema.sql", StringComparison.OrdinalIgnoreCase));
        using Stream stream = asm.GetManifestResourceStream(resource)!;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
