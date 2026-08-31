using System;
using System.Collections.Generic;

namespace PwdManager.Infrastructure.Persistence.Entities;

public partial class Category
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public long? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? DeletedBy { get; set; }

    public virtual ICollection<CategoryPermission> CategoryPermissions { get; set; } = new List<CategoryPermission>();

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? DeletedByNavigation { get; set; }

    public virtual ICollection<Secret> Secrets { get; set; } = new List<Secret>();
}
