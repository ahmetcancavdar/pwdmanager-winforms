using Microsoft.Extensions.DependencyInjection;
using PwdManager.Application;
using PwdManager.Application.Configuration;
using PwdManager.Application.Interfaces;
using PwdManager.Infrastructure;
using PwdManager.WinForms.Forms;

namespace PwdManager.WinForms.Composition;

/// <summary>
/// Composition root. Wires the layered DI extensions (<c>AddApplication</c> +
/// <c>AddInfrastructure</c>) and registers the WinForms top-level windows.
/// </summary>
public static class AppServices
{
    public static ServiceProvider Build(AppConfig config)
    {
        var services = new ServiceCollection();

        services.AddApplication(config);
        services.AddInfrastructure(config);

        services.AddTransient<LoginForm>();
        services.AddTransient<ChangePasswordForm>();
        services.AddTransient<AdminShellForm>();
        services.AddTransient<PersonnelShellForm>();

        return services.BuildServiceProvider();
    }
}

/// <summary>
/// Pre-DI bootstrap helpers. Needed before the service provider exists (first-run
/// wizard, readiness probe), so the concrete Infrastructure types are named here —
/// the one spot in WinForms outside <see cref="AppServices"/> that does.
/// </summary>
public static class Bootstrap
{
    public static IConfigStore ConfigStore()
        => new PwdManager.Infrastructure.Configuration.ConfigStore();

    public static IDatabaseBootstrapper DatabaseBootstrapper()
        => new PwdManager.Infrastructure.Configuration.DatabaseBootstrapper();
}

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
