using System;
using System.Collections.Generic;

namespace PwdManager.Infrastructure.Persistence.Entities;

public partial class SecretDeny
{
    public long UserId { get; set; }

    public long SecretId { get; set; }

    public long? DeniedBy { get; set; }

    public DateTime DeniedAt { get; set; }

    public virtual User? DeniedByNavigation { get; set; }

    public virtual Secret Secret { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
