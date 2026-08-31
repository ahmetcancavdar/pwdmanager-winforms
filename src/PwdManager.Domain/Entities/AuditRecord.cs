namespace PwdManager.Domain.Entities;

/// <summary>One row of the audit trail as the domain sees it.</summary>
public sealed record AuditRecord(
    DateTime CreatedAt, string Username, string Action, string TargetType, long? TargetId, string Detail);
