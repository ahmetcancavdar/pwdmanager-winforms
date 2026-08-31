using System;
using System.Collections.Generic;

namespace PwdManager.Infrastructure.Persistence.Entities;

public partial class PermissionSync
{
    public long UserId { get; set; }

    public long Version { get; set; }

    public virtual User User { get; set; } = null!;
}
