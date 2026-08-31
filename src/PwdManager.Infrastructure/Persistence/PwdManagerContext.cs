using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PwdManager.Infrastructure.Persistence.Entities;

namespace PwdManager.Infrastructure.Persistence;

public partial class PwdManagerContext : DbContext
{
    public PwdManagerContext(DbContextOptions<PwdManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppMetum> AppMeta { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryPermission> CategoryPermissions { get; set; }

    public virtual DbSet<PermissionSync> PermissionSyncs { get; set; }

    public virtual DbSet<Secret> Secrets { get; set; }

    public virtual DbSet<SecretDeny> SecretDenies { get; set; }

    public virtual DbSet<SecretPermission> SecretPermissions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<AppMetum>(entity =>
        {
            entity.HasKey(e => e.MetaKey).HasName("PRIMARY");

            entity.ToTable("app_meta");

            entity.Property(e => e.MetaKey)
                .HasMaxLength(64)
                .HasColumnName("meta_key");
            entity.Property(e => e.MetaValue)
                .HasMaxLength(1024)
                .HasColumnName("meta_value");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("audit_log");

            entity.HasIndex(e => e.CreatedAt, "ix_audit_created");

            entity.HasIndex(e => e.UserId, "ix_audit_user");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.Action)
                .HasMaxLength(48)
                .HasColumnName("action");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Detail)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("detail");
            entity.Property(e => e.TargetId)
                .HasColumnType("bigint(20)")
                .HasColumnName("target_id");
            entity.Property(e => e.TargetType)
                .HasMaxLength(32)
                .HasDefaultValueSql("''")
                .HasColumnName("target_type");
            entity.Property(e => e.UserId)
                .HasColumnType("bigint(20)")
                .HasColumnName("user_id");
            entity.Property(e => e.Username)
                .HasMaxLength(64)
                .HasDefaultValueSql("''")
                .HasColumnName("username");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("categories");

            entity.HasIndex(e => e.DeletedBy, "fk_categories_deleter");

            entity.HasIndex(e => e.CreatedBy, "fk_categories_user");

            entity.HasIndex(e => e.DeletedAt, "ix_categories_deleted");

            entity.HasIndex(e => e.Name, "ix_categories_name");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasColumnType("bigint(20)")
                .HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy)
                .HasColumnType("bigint(20)")
                .HasColumnName("deleted_by");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasDefaultValueSql("''")
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.CategoryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_categories_user");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.CategoryDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_categories_deleter");
        });

        modelBuilder.Entity<CategoryPermission>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.CategoryId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("category_permissions");

            entity.HasIndex(e => e.CategoryId, "fk_cp_category");

            entity.HasIndex(e => e.GrantedBy, "fk_cp_granter");

            entity.Property(e => e.UserId)
                .HasColumnType("bigint(20)")
                .HasColumnName("user_id");
            entity.Property(e => e.CategoryId)
                .HasColumnType("bigint(20)")
                .HasColumnName("category_id");
            entity.Property(e => e.GrantedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("granted_at");
            entity.Property(e => e.GrantedBy)
                .HasColumnType("bigint(20)")
                .HasColumnName("granted_by");

            entity.HasOne(d => d.Category).WithMany(p => p.CategoryPermissions)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("fk_cp_category");

            entity.HasOne(d => d.GrantedByNavigation).WithMany(p => p.CategoryPermissionGrantedByNavigations)
                .HasForeignKey(d => d.GrantedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_cp_granter");

            entity.HasOne(d => d.User).WithMany(p => p.CategoryPermissionUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_cp_user");
        });

        modelBuilder.Entity<PermissionSync>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("permission_sync");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnType("bigint(20)")
                .HasColumnName("user_id");
            entity.Property(e => e.Version)
                .HasColumnType("bigint(20)")
                .HasColumnName("version");

            entity.HasOne(d => d.User).WithOne(p => p.PermissionSync)
                .HasForeignKey<PermissionSync>(d => d.UserId)
                .HasConstraintName("fk_ps_user");
        });

        modelBuilder.Entity<Secret>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("secrets");

            entity.HasIndex(e => e.DeletedBy, "fk_secrets_deleter");

            entity.HasIndex(e => e.CreatedBy, "fk_secrets_user");

            entity.HasIndex(e => e.CategoryId, "ix_secrets_category");

            entity.HasIndex(e => e.DeletedAt, "ix_secrets_deleted");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.CategoryId)
                .HasColumnType("bigint(20)")
                .HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.CreatedBy)
                .HasColumnType("bigint(20)")
                .HasColumnName("created_by");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.DeletedBy)
                .HasColumnType("bigint(20)")
                .HasColumnName("deleted_by");
            entity.Property(e => e.Notes)
                .HasMaxLength(512)
                .HasDefaultValueSql("''")
                .HasColumnName("notes");
            entity.Property(e => e.SecretCipher)
                .HasMaxLength(4096)
                .HasColumnName("secret_cipher");
            entity.Property(e => e.Title)
                .HasMaxLength(128)
                .HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.UsernameCipher)
                .HasMaxLength(1024)
                .HasColumnName("username_cipher");

            entity.HasOne(d => d.Category).WithMany(p => p.Secrets)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("fk_secrets_category");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SecretCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_secrets_user");

            entity.HasOne(d => d.DeletedByNavigation).WithMany(p => p.SecretDeletedByNavigations)
                .HasForeignKey(d => d.DeletedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_secrets_deleter");
        });

        modelBuilder.Entity<SecretDeny>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.SecretId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("secret_denies");

            entity.HasIndex(e => e.DeniedBy, "fk_sd_denier");

            entity.HasIndex(e => e.SecretId, "fk_sd_secret");

            entity.Property(e => e.UserId)
                .HasColumnType("bigint(20)")
                .HasColumnName("user_id");
            entity.Property(e => e.SecretId)
                .HasColumnType("bigint(20)")
                .HasColumnName("secret_id");
            entity.Property(e => e.DeniedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("denied_at");
            entity.Property(e => e.DeniedBy)
                .HasColumnType("bigint(20)")
                .HasColumnName("denied_by");

            entity.HasOne(d => d.DeniedByNavigation).WithMany(p => p.SecretDenyDeniedByNavigations)
                .HasForeignKey(d => d.DeniedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_sd_denier");

            entity.HasOne(d => d.Secret).WithMany(p => p.SecretDenies)
                .HasForeignKey(d => d.SecretId)
                .HasConstraintName("fk_sd_secret");

            entity.HasOne(d => d.User).WithMany(p => p.SecretDenyUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_sd_user");
        });

        modelBuilder.Entity<SecretPermission>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.SecretId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("secret_permissions");

            entity.HasIndex(e => e.GrantedBy, "fk_sp_granter");

            entity.HasIndex(e => e.SecretId, "fk_sp_secret");

            entity.Property(e => e.UserId)
                .HasColumnType("bigint(20)")
                .HasColumnName("user_id");
            entity.Property(e => e.SecretId)
                .HasColumnType("bigint(20)")
                .HasColumnName("secret_id");
            entity.Property(e => e.GrantedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("granted_at");
            entity.Property(e => e.GrantedBy)
                .HasColumnType("bigint(20)")
                .HasColumnName("granted_by");

            entity.HasOne(d => d.GrantedByNavigation).WithMany(p => p.SecretPermissionGrantedByNavigations)
                .HasForeignKey(d => d.GrantedBy)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_sp_granter");

            entity.HasOne(d => d.Secret).WithMany(p => p.SecretPermissions)
                .HasForeignKey(d => d.SecretId)
                .HasConstraintName("fk_sp_secret");

            entity.HasOne(d => d.User).WithMany(p => p.SecretPermissionUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_sp_user");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Username, "uq_users_username").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.FailedLoginCount)
                .HasColumnType("int(11)")
                .HasColumnName("failed_login_count");
            entity.Property(e => e.FullName)
                .HasMaxLength(128)
                .HasDefaultValueSql("''")
                .HasColumnName("full_name");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_active");
            entity.Property(e => e.KdfSalt)
                .HasMaxLength(16)
                .HasColumnName("kdf_salt");
            entity.Property(e => e.LockedUntil)
                .HasColumnType("datetime")
                .HasColumnName("locked_until");
            entity.Property(e => e.MustChangePw)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("must_change_pw");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.Role)
                .HasColumnType("enum('Admin','Personnel')")
                .HasColumnName("role");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("updated_at");
            entity.Property(e => e.Username)
                .HasMaxLength(64)
                .HasColumnName("username");
            entity.Property(e => e.WrappedDek)
                .HasMaxLength(64)
                .HasColumnName("wrapped_dek");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
