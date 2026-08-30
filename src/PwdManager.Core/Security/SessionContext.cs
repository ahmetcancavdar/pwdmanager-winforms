using PwdManager.Core.Models;

namespace PwdManager.Core.Security;

/// <summary>
/// The signed-in user plus the live <see cref="SecretProtector"/> (DEK) for this session.
/// Created by the login flow, disposed on logout / app exit.
/// </summary>
public sealed class SessionContext : IDisposable
{
    public AuthenticatedUser User { get; }
    public SecretProtector Protector { get; }

    /// <summary>Last permission_sync version this client has applied (personnel only).</summary>
    public long LastPermissionVersion { get; set; }

    public SessionContext(AuthenticatedUser user, byte[] dek)
    {
        User = user;
        Protector = new SecretProtector(dek);
    }

    public bool IsAdmin => User.Role == UserRole.Admin;

    /// <summary>Defense in depth: throws unless this session belongs to an admin.</summary>
    public void EnsureAdmin()
    {
        if (!IsAdmin)
            throw new InvalidOperationException("Bu işlem yalnızca yönetici tarafından yapılabilir.");
    }

    public void Dispose() => Protector.Clear();
}
