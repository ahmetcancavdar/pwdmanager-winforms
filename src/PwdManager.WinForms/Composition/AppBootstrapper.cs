using PwdManager.Application.Configuration;

namespace PwdManager.WinForms.Composition;

public static class AppBootstrapper
{
    /// <summary>
    /// True when the app is fully initialised: local config present, server reachable,
    /// schema applied, and at least one admin exists. Otherwise the setup wizard must run.
    /// </summary>
    public static async Task<bool> IsReadyAsync(AppConfig config)
    {
        var configStore = Bootstrap.ConfigStore();
        if (!configStore.LocalConfigExists() || string.IsNullOrWhiteSpace(config.Database.User))
            return false;

        try
        {
            var db = Bootstrap.DatabaseBootstrapper();
            await db.TestConnectionAsync(config.Database);

            if (!await db.SchemaExistsAsync(config.Database) || !await db.HasAdminAsync(config.Database))
                return false;

            // schema.sql is idempotent (CREATE ... IF NOT EXISTS); re-applying it on every
            // start picks up additive schema changes (e.g. new tables) without a wizard run.
            await db.ApplySchemaAsync(config.Database);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
