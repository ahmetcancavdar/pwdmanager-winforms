using PwdManager.Application.Configuration;
using PwdManager.Application.Interfaces;
using PwdManager.Application.Models;
using PwdManager.Domain.Security;

namespace PwdManager.Application.Services;

/// <summary>
/// Add/update/reveal for stored credentials. All encryption/decryption uses the
/// session DEK via <see cref="SessionContext.Protector"/>; reveal re-checks permission
/// authoritatively against the database (admins bypass the check).
/// </summary>
public sealed class SecretService
{
    private readonly ISecretRepository _secrets;
    private readonly IPermissionRepository _permissions;
    private readonly IUserRepository _users;
    private readonly IAuditRepository _audit;
    private readonly IRevealLockRepository _revealLocks;
    private readonly SecurityConfig _security;

    public SecretService(ISecretRepository secrets, IPermissionRepository permissions,
        IUserRepository users, IAuditRepository audit, IRevealLockRepository revealLocks, SecurityConfig security)
    {
        _secrets = secrets;
        _permissions = permissions;
        _users = users;
        _audit = audit;
        _revealLocks = revealLocks;
        _security = security;
    }

    public async Task<long> AddAsync(SessionContext s, long categoryId, string title,
        string username, string password, string notes, CancellationToken ct = default)
    {
        s.EnsureAdmin();
        string cleanTitle = title.Trim();
        var request = new NewSecret(
            CategoryId: categoryId,
            Title: cleanTitle,
            UsernameCipher: string.IsNullOrEmpty(username) ? null : s.Protector.Protect(username),
            SecretCipher: s.Protector.Protect(password),
            Notes: notes ?? "",
            CreatedBy: s.User.Id);

        long id = await _secrets.CreateAsync(request, ct);
        await _audit.WriteAsync(AuditAction.SecretAdd, s.User.Id, s.User.Username, "secret", id, cleanTitle, ct);
        return id;
    }

    public async Task UpdateAsync(SessionContext s, long id, long categoryId, string title,
        string username, string password, string notes, CancellationToken ct = default)
    {
        s.EnsureAdmin();
        var existing = await _secrets.GetByIdAsync(id, ct)
                       ?? throw new InvalidOperationException("Kayıt bulunamadı.");

        string cleanTitle = title.Trim();
        await _secrets.UpdateAsync(
            existing.Id,
            categoryId,
            cleanTitle,
            string.IsNullOrEmpty(username) ? null : s.Protector.Protect(username),
            s.Protector.Protect(password),
            notes ?? "",
            ct);
        await _audit.WriteAsync(AuditAction.SecretEdit, s.User.Id, s.User.Username, "secret", id, cleanTitle, ct);
    }

    /// <summary>Soft delete → "Silinenler"e taşır; kayıt DB'de kalır, geri yüklenebilir.</summary>
    public async Task DeleteAsync(SessionContext s, long id, string title, CancellationToken ct = default)
    {
        s.EnsureAdmin();
        await _secrets.SoftDeleteAsync(id, s.User.Id, ct);
        await _audit.WriteAsync(AuditAction.SecretDelete, s.User.Id, s.User.Username, "secret", id, title, ct);
    }

    /// <summary>
    /// Authoritative "may this session still see this secret" check. Admins always can;
    /// a personnel loses access the instant the admin revokes the grant. Used both before
    /// a reveal and repeatedly while a reveal window is open.
    /// </summary>
    public async Task<bool> CanRevealAsync(SessionContext s, long secretId, CancellationToken ct = default)
    {
        if (s.IsAdmin)
            return true;
        // Deactivated account -> no access, even mid-view.
        if (!await _users.IsActiveAsync(s.User.Id, ct))
            return false;
        return await _permissions.CanViewSecretAsync(s.User.Id, secretId, ct);
    }

    /// <summary>Returns the decrypted credential, or null if the user is not (or no longer) permitted.</summary>
    public async Task<RevealedSecret?> RevealAsync(SessionContext s, long secretId, CancellationToken ct = default)
    {
        if (!s.IsAdmin && !await _permissions.CanViewSecretAsync(s.User.Id, secretId, ct))
        {
            await _audit.WriteAsync(AuditAction.SecretViewDenied, s.User.Id, s.User.Username, "secret", secretId, ct: ct);
            return null;
        }

        var entity = await _secrets.GetByIdAsync(secretId, ct);
        if (entity is null) return null;

        var revealed = new RevealedSecret
        {
            Title = entity.Title,
            Username = entity.UsernameCipher is null ? "" : s.Protector.Unprotect(entity.UsernameCipher),
            Password = s.Protector.Unprotect(entity.SecretCipher),
            Notes = entity.Notes
        };
        await _audit.WriteAsync(AuditAction.SecretView, s.User.Id, s.User.Username, "secret", secretId, entity.Title, ct);
        return revealed;
    }

    /// <summary>
    /// Reveal re-authentication lockout for this personnel + this specific secret,
    /// independent of the account-wide login lockout. Kept in the database so it
    /// cannot be bypassed by refreshing the list or reopening the row.
    /// </summary>
    public async Task<DateTime?> GetRevealLockAsync(SessionContext s, long secretId, CancellationToken ct = default)
    {
        if (s.IsAdmin) return null;
        var (_, lockedUntil) = await _revealLocks.GetAsync(s.User.Id, secretId, ct);
        return lockedUntil is { } lu && lu > DateTime.UtcNow ? lu : null;
    }

    /// <summary>
    /// Records one failed reveal re-auth attempt. Returns the remaining attempts, or
    /// (0, lockedUntil) once <see cref="SecurityConfig.RevealMaxAttempts"/> is reached.
    /// </summary>
    public async Task<(int RemainingAttempts, DateTime? LockedUntil)> RegisterRevealFailureAsync(
        SessionContext s, long secretId, CancellationToken ct = default)
    {
        if (s.IsAdmin) return (int.MaxValue, null);

        var (failedCount, lockedUntil) = await _revealLocks.RegisterFailureAsync(
            s.User.Id, secretId, _security.RevealMaxAttempts, _security.RevealLockoutMinutes, ct);

        if (lockedUntil is { } lu && lu > DateTime.UtcNow)
        {
            await _audit.WriteAsync(AuditAction.RevealAuthFailed, s.User.Id, s.User.Username, "secret", secretId,
                detail: $"locked {_security.RevealLockoutMinutes}dk", ct: ct);
            return (0, lu);
        }

        return (Math.Max(0, _security.RevealMaxAttempts - failedCount), null);
    }

    /// <summary>Clears the reveal-lock counter after a successful re-auth.</summary>
    public async Task ClearRevealLockAsync(SessionContext s, long secretId, CancellationToken ct = default)
    {
        if (s.IsAdmin) return;
        await _revealLocks.ClearAsync(s.User.Id, secretId, ct);
    }
}
