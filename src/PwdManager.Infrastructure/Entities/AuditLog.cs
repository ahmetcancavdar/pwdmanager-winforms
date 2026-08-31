using System;
using System.Collections.Generic;

namespace PwdManager.Infrastructure.Entities;

public partial class AuditLog
{
    public long Id { get; set; }

    public long? UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Action { get; set; } = null!;

    public string TargetType { get; set; } = null!;

    public long? TargetId { get; set; }

    public string Detail { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
