using PwdManager.Application.Configuration;

namespace PwdManager.Application.Interfaces;

/// <summary>
/// Loads/saves application settings. The DB password is stored encrypted at rest
/// (Windows DPAPI) by the implementation — Application never sees the mechanism.
/// </summary>
public interface IConfigStore
{
    AppConfig Load();
    void SaveDatabase(DatabaseConfig database);
    bool LocalConfigExists();
}

/// <summary>
/// First-run database checks/creation, done with a raw connection (works before the
/// EF context and its database exist).
/// </summary>
public interface IDatabaseBootstrapper
{
    Task TestConnectionAsync(DatabaseConfig database, CancellationToken ct = default);
    Task<bool> SchemaExistsAsync(DatabaseConfig database, CancellationToken ct = default);
    Task<bool> HasAdminAsync(DatabaseConfig database, CancellationToken ct = default);

    /// <summary>Create the database + tables. Idempotent; needs DDL rights.</summary>
    Task ApplySchemaAsync(DatabaseConfig database, CancellationToken ct = default);
}
