using System;
using System.Collections.Generic;

namespace PwdManager.Infrastructure.Persistence.Entities;

public partial class User
{
    public long Id { get; set; }

    public string Username { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public byte[] KdfSalt { get; set; } = null!;

    public byte[] WrappedDek { get; set; } = null!;

    public bool? IsActive { get; set; }

    public bool? MustChangePw { get; set; }

    public int FailedLoginCount { get; set; }

    public DateTime? LockedUntil { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Category> CategoryCreatedByNavigations { get; set; } = new List<Category>();

    public virtual ICollection<Category> CategoryDeletedByNavigations { get; set; } = new List<Category>();

    public virtual ICollection<CategoryPermission> CategoryPermissionGrantedByNavigations { get; set; } = new List<CategoryPermission>();

    public virtual ICollection<CategoryPermission> CategoryPermissionUsers { get; set; } = new List<CategoryPermission>();

    public virtual PermissionSync? PermissionSync { get; set; }

    public virtual ICollection<Secret> SecretCreatedByNavigations { get; set; } = new List<Secret>();

    public virtual ICollection<Secret> SecretDeletedByNavigations { get; set; } = new List<Secret>();

    public virtual ICollection<SecretDeny> SecretDenyDeniedByNavigations { get; set; } = new List<SecretDeny>();

    public virtual ICollection<SecretDeny> SecretDenyUsers { get; set; } = new List<SecretDeny>();

    public virtual ICollection<SecretPermission> SecretPermissionGrantedByNavigations { get; set; } = new List<SecretPermission>();

    public virtual ICollection<SecretPermission> SecretPermissionUsers { get; set; } = new List<SecretPermission>();
}
