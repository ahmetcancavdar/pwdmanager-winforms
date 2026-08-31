using PwdManager.Application.Models;
using PwdManager.Domain.Enums;
using PwdManager.Infrastructure.Entities;

namespace PwdManager.Infrastructure.Repositories;

/// <summary>EF entity → Application record. Keeps the persistence model out of Application.</summary>
internal static class Map
{
    public static UserRole ToRole(string role) =>
        role == nameof(UserRole.Admin) ? UserRole.Admin : UserRole.Personnel;

    public static UserRecord ToRecord(this User u) => new(
        u.Id, u.Username, u.FullName, ToRole(u.Role),
        u.PasswordHash, u.KdfSalt, u.WrappedDek,
        u.IsActive == true, u.MustChangePw == true, u.FailedLoginCount, u.LockedUntil, u.CreatedAt);

    public static CategoryRecord ToRecord(this Category c) => new(
        c.Id, c.Name, c.Description, c.CreatedBy, c.CreatedAt, c.DeletedAt, c.DeletedBy);

    public static SecretRecord ToRecord(this Secret s) => new(
        s.Id, s.CategoryId, s.Title, s.UsernameCipher, s.SecretCipher, s.Notes,
        s.CreatedAt, s.UpdatedAt, s.DeletedAt);

    public static AuditRecord ToRecord(this AuditLog a) => new(
        a.CreatedAt, a.Username, a.Action, a.TargetType, a.TargetId, a.Detail);
}
