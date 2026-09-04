using System;
using System.Collections.Generic;

namespace PwdManager.Infrastructure.Entities;

public partial class SecretRevealLock
{
    public long UserId { get; set; }

    public long SecretId { get; set; }

    public int FailedCount { get; set; }

    public DateTime? LockedUntil { get; set; }
}
