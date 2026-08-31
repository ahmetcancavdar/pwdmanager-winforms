using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PwdManager.Application.Configuration;
using PwdManager.Application.Interfaces;
using PwdManager.Domain.Security;
using PwdManager.Infrastructure.Configuration;
using PwdManager.Infrastructure.Persistence;
using PwdManager.Infrastructure.Repositories;
using PwdManager.Infrastructure.Security;

namespace PwdManager.Infrastructure;

/// <summary>
/// Wires the concrete persistence + crypto + bootstrap implementations to the
/// abstractions declared in <c>PwdManager.Application</c>. This is the only place
/// EF Core / Pomelo / DPAPI are named.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, AppConfig config)
    {
        string connectionString = DbConnection.BuildConnectionString(config.Database, includeDatabase: true);
        services.AddDbContextFactory<PwdManagerContext>(options =>
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

        services.AddSingleton<IUserRepository, UserRepository>();
        services.AddSingleton<ICategoryRepository, CategoryRepository>();
        services.AddSingleton<ISecretRepository, SecretRepository>();
        services.AddSingleton<IPermissionRepository, PermissionRepository>();
        services.AddSingleton<IAuditRepository, AuditRepository>();
        services.AddSingleton<IAppMetaRepository, AppMetaRepository>();

        services.AddSingleton<IPasswordHasher, Argon2PasswordHasher>();
        services.AddSingleton<IKeyDerivation, Argon2KeyDerivation>();
        services.AddSingleton<IDataProtector, AesGcmDataProtector>();
        services.AddSingleton<IRecoveryCodeService, RecoveryCodeService>();

        services.AddSingleton<IConfigStore, ConfigStore>();
        services.AddSingleton<IDatabaseBootstrapper, DatabaseBootstrapper>();

        return services;
    }
}
