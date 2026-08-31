using System;
using System.Collections.Generic;

namespace PwdManager.Infrastructure.Persistence.Entities;

public partial class CategoryPermission
{
    public long UserId { get; set; }

    public long CategoryId { get; set; }

    public long? GrantedBy { get; set; }

    public DateTime GrantedAt { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual User? GrantedByNavigation { get; set; }

    public virtual User User { get; set; } = null!;
}
