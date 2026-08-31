using System;
using System.Collections.Generic;

namespace PwdManager.Infrastructure.Entities;

public partial class SecretPermission
{
    public long UserId { get; set; }

    public long SecretId { get; set; }

    public long? GrantedBy { get; set; }

    public DateTime GrantedAt { get; set; }

    public virtual User? GrantedByNavigation { get; set; }

    public virtual Secret Secret { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
