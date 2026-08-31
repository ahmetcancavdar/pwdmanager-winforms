using System.Reflection;
using MySqlConnector;
using PwdManager.Application.Configuration;
using PwdManager.Application.Interfaces;

namespace PwdManager.Infrastructure.Configuration;

/// <summary>
/// First-run helpers used by the setup wizard: test the server, apply the schema,
/// and report whether the app has been initialised yet. Raw ADO.NET so it works
/// before the EF <c>DbContext</c> (and its database) exist.
/// </summary>
public sealed class DatabaseBootstrapper : IDatabaseBootstrapper
{
    public async Task TestConnectionAsync(DatabaseConfig db, CancellationToken ct = default)
    {
        await using var c = new MySqlConnection(DbConnection.BuildConnectionString(db, includeDatabase: false));
        await c.OpenAsync(ct);
    }

    public async Task<bool> SchemaExistsAsync(DatabaseConfig db, CancellationToken ct = default)
    {
        await using var c = new MySqlConnection(DbConnection.BuildConnectionString(db, includeDatabase: false));
        await c.OpenAsync(ct);
        await using var cmd = new MySqlCommand(
            @"SELECT COUNT(*) FROM information_schema.tables
              WHERE table_schema = @db AND table_name = 'users'", c);
        cmd.Parameters.AddWithValue("@db", db.Name);
        return Convert.ToInt64(await cmd.ExecuteScalarAsync(ct)) > 0;
    }

    public async Task<bool> HasAdminAsync(DatabaseConfig db, CancellationToken ct = default)
    {
        await using var c = new MySqlConnection(DbConnection.BuildConnectionString(db, includeDatabase: true));
        await c.OpenAsync(ct);
        await using var cmd = new MySqlCommand("SELECT COUNT(*) FROM users WHERE role = 'Admin'", c);
        return Convert.ToInt64(await cmd.ExecuteScalarAsync(ct)) > 0;
    }

    public async Task ApplySchemaAsync(DatabaseConfig db, CancellationToken ct = default)
    {
        string script = LoadSchemaScript();
        await using var c = new MySqlConnection(DbConnection.BuildConnectionString(db, includeDatabase: false));
        await c.OpenAsync(ct);
        await using var cmd = new MySqlCommand(script, c);
        await cmd.ExecuteNonQueryAsync(ct);
    }

    private static string LoadSchemaScript()
    {
        Assembly asm = typeof(DatabaseBootstrapper).Assembly;
        string resource = asm.GetManifestResourceNames()
            .Single(n => n.EndsWith("schema.sql", StringComparison.OrdinalIgnoreCase));
        using Stream stream = asm.GetManifestResourceStream(resource)!;
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
