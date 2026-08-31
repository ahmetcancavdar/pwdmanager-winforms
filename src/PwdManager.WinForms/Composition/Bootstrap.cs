using PwdManager.Application.Interfaces;

namespace PwdManager.WinForms.Composition;

/// <summary>
/// Pre-DI bootstrap helpers. These are needed before the service provider exists
/// (first-run wizard, readiness probe), so the concrete Infrastructure types are
/// named here — the one place in WinForms outside the composition root that does.
/// </summary>
public static class Bootstrap
{
    public static IConfigStore ConfigStore()
        => new PwdManager.Infrastructure.Configuration.ConfigStore();

    public static IDatabaseBootstrapper DatabaseBootstrapper()
        => new PwdManager.Infrastructure.Configuration.DatabaseBootstrapper();
}
