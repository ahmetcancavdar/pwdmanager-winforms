using PwdManager.Domain.Entities;

namespace PwdManager.Application.Interfaces;

public interface IAuditRepository
{
    Task WriteAsync(string action, long? userId, string username,
        string targetType = "", long? targetId = null, string detail = "", CancellationToken ct = default);

    Task<IReadOnlyList<AuditRecord>> RecentAsync(int limit, CancellationToken ct = default);
}
