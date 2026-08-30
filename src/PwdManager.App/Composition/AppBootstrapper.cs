using PwdManager.Data.Infrastructure;

namespace PwdManager.App.Composition;

public static class AppBootstrapper
{
    /// <summary>
    /// True when the app is fully initialised: local config present, server reachable,
    /// schema applied, and at least one admin exists. Otherwise the setup wizard must run.
    /// </summary>
    public static async Task<bool> IsReadyAsync(AppConfig config)
    {
        if (!ConfigStore.LocalConfigExists() || string.IsNullOrWhiteSpace(config.Database.User))
            return false;

        try
        {
            var installer = new DatabaseInstaller(config.ToDbOptions());
            await installer.TestServerConnectionAsync();
            return await installer.SchemaExistsAsync() && await installer.HasAdminAsync();
        }
        catch
        {
            return false;
        }
    }
}
