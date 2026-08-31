using System;
using System.Collections.Generic;

namespace PwdManager.Infrastructure.Persistence.Entities;

public partial class Secret
{
    public long Id { get; set; }

    public long CategoryId { get; set; }

    public string Title { get; set; } = null!;

    public byte[]? UsernameCipher { get; set; }

    public byte[] SecretCipher { get; set; } = null!;

    public string Notes { get; set; } = null!;

    public long? CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? DeletedBy { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? DeletedByNavigation { get; set; }

    public virtual ICollection<SecretDeny> SecretDenies { get; set; } = new List<SecretDeny>();

    public virtual ICollection<SecretPermission> SecretPermissions { get; set; } = new List<SecretPermission>();
}
