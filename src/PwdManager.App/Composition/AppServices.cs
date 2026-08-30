using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PwdManager.App.Forms;
using PwdManager.App.Services;
using PwdManager.Data.Persistence;
using PwdManager.Data.Repositories;

namespace PwdManager.App.Composition;

/// <summary>Builds the application service provider once the database is known to be ready.</summary>
public static class AppServices
{
    public static ServiceProvider Build(AppConfig config)
    {
        var services = new ServiceCollection();

        services.AddSingleton(config);
        services.AddSingleton(config.Security);

        var dbOptions = config.ToDbOptions();
        services.AddSingleton(dbOptions);

        string connectionString = dbOptions.BuildConnectionString(includeDatabase: true);
        services.AddDbContextFactory<PwdManagerContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<ICategoryRepository, CategoryRepository>();
        services.AddSingleton<ISecretRepository, SecretRepository>();
        services.AddSingleton<IPermissionRepository, PermissionRepository>();
        services.AddSingleton<IAuditRepository, AuditRepository>();
        services.AddSingleton<IAppMetaRepository, AppMetaRepository>();

        services.AddSingleton<AuthService>();
        services.AddSingleton<SetupService>();
        services.AddSingleton<SecretService>();
        services.AddSingleton<PersonnelService>();
        services.AddSingleton<CategoryService>();
        services.AddSingleton<PermissionService>();

        services.AddTransient<LoginForm>();
        services.AddTransient<ChangePasswordForm>();
        services.AddTransient<AdminShellForm>();
        services.AddTransient<PersonnelShellForm>();

        return services.BuildServiceProvider();
    }
}
