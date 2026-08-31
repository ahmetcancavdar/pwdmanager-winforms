using PwdManager.Application.Interfaces;
using PwdManager.Application.DTOs;
using PwdManager.Application.Models;
using PwdManager.Application.Security;

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

    public SecretService(ISecretRepository secrets, IPermissionRepository permissions,
        IUserRepository users, IAuditRepository audit)
    {
        _secrets = secrets;
        _permissions = permissions;
        _users = users;
        _audit = audit;
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
}
