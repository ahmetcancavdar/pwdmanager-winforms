using Microsoft.Extensions.DependencyInjection;
using PwdManager.Application;
using PwdManager.Application.Configuration;
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
