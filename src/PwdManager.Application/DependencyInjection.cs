using Microsoft.Extensions.DependencyInjection;
using PwdManager.Application.Configuration;
using PwdManager.Application.Services;

namespace PwdManager.Application;

/// <summary>
/// Registers the use-case services. Depends only on abstractions declared in this
/// assembly (repository + crypto interfaces); the implementations come from
/// <c>AddInfrastructure</c> in the composition root.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, AppConfig config)
    {
        services.AddSingleton(config);
        services.AddSingleton(config.Security);

        services.AddSingleton<AuthService>();
        services.AddSingleton<SetupService>();
        services.AddSingleton<SecretService>();
        services.AddSingleton<PersonnelService>();
        services.AddSingleton<CategoryService>();
        services.AddSingleton<PermissionService>();
        services.AddSingleton<TrashService>();

        return services;
    }
}
