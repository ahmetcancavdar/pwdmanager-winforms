-- PwdManager schema. MySQL 8.x / InnoDB / utf8mb4.
-- Executed by the first-run setup wizard. Idempotent (IF NOT EXISTS).

CREATE DATABASE IF NOT EXISTS `pwdmanager`
  CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE `pwdmanager`;

-- ---------------------------------------------------------------------------
-- users: accounts for both roles.
--   password_hash : Argon2id encoded string, used for login verification
--   kdf_salt      : salt for deriving the per-user KEK from the login password
--   wrapped_dek   : AES-256-GCM(KEK, DEK) = nonce(12) || ciphertext(32) || tag(16)
-- ---------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `users` (
  `id`                 BIGINT NOT NULL AUTO_INCREMENT,
  `username`           VARCHAR(64)   NOT NULL,
  `full_name`          VARCHAR(128)  NOT NULL DEFAULT '',
  `role`               ENUM('Admin','Personnel') NOT NULL,
  `password_hash`      VARCHAR(255)  NOT NULL,
  `kdf_salt`           VARBINARY(16) NOT NULL,
  `wrapped_dek`        VARBINARY(64) NOT NULL,
  `is_active`          TINYINT(1)    NOT NULL DEFAULT 1,
  `must_change_pw`     TINYINT(1)    NOT NULL DEFAULT 1,
  `failed_login_count` INT           NOT NULL DEFAULT 0,
  `locked_until`       DATETIME      NULL,
  `created_at`         DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at`         DATETIME      NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uq_users_username` (`username`)
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------------
-- categories
-- ---------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `categories` (
  `id`          BIGINT NOT NULL AUTO_INCREMENT,
  `name`        VARCHAR(128) NOT NULL,
  `description` VARCHAR(255) NOT NULL DEFAULT '',
  `created_by`  BIGINT NULL,
  `created_at`  DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uq_categories_name` (`name`),
  CONSTRAINT `fk_categories_user` FOREIGN KEY (`created_by`)
    REFERENCES `users` (`id`) ON DELETE SET NULL
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------------
-- secrets: one stored credential. username & secret are AES-256-GCM blobs
--   blob layout = nonce(12) || ciphertext(n) || tag(16)
-- ---------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `secrets` (
  `id`              BIGINT NOT NULL AUTO_INCREMENT,
  `category_id`     BIGINT NOT NULL,
  `title`           VARCHAR(128) NOT NULL,
  `username_cipher` VARBINARY(1024) NULL,
  `secret_cipher`   VARBINARY(4096) NOT NULL,
  `notes`           VARCHAR(512) NOT NULL DEFAULT '',
  `created_by`      BIGINT NULL,
  `created_at`      DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `updated_at`      DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `ix_secrets_category` (`category_id`),
  CONSTRAINT `fk_secrets_category` FOREIGN KEY (`category_id`)
    REFERENCES `categories` (`id`) ON DELETE CASCADE,
  CONSTRAINT `fk_secrets_user` FOREIGN KEY (`created_by`)
    REFERENCES `users` (`id`) ON DELETE SET NULL
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------------
-- category_permissions: grant a personnel user access to an ENTIRE category
-- ---------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `category_permissions` (
  `user_id`     BIGINT NOT NULL,
  `category_id` BIGINT NOT NULL,
  `granted_by`  BIGINT NULL,
  `granted_at`  DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`user_id`, `category_id`),
  CONSTRAINT `fk_cp_user` FOREIGN KEY (`user_id`)
    REFERENCES `users` (`id`) ON DELETE CASCADE,
  CONSTRAINT `fk_cp_category` FOREIGN KEY (`category_id`)
    REFERENCES `categories` (`id`) ON DELETE CASCADE,
  CONSTRAINT `fk_cp_granter` FOREIGN KEY (`granted_by`)
    REFERENCES `users` (`id`) ON DELETE SET NULL
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------------
-- secret_permissions: grant access to a SINGLE secret inside a category
-- ---------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `secret_permissions` (
  `user_id`    BIGINT NOT NULL,
  `secret_id`  BIGINT NOT NULL,
  `granted_by` BIGINT NULL,
  `granted_at` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`user_id`, `secret_id`),
  CONSTRAINT `fk_sp_user` FOREIGN KEY (`user_id`)
    REFERENCES `users` (`id`) ON DELETE CASCADE,
  CONSTRAINT `fk_sp_secret` FOREIGN KEY (`secret_id`)
    REFERENCES `secrets` (`id`) ON DELETE CASCADE,
  CONSTRAINT `fk_sp_granter` FOREIGN KEY (`granted_by`)
    REFERENCES `users` (`id`) ON DELETE SET NULL
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------------
-- permission_sync: bumped on every grant/revoke for a user. The personnel
-- client polls this cheap value; a change means "rebuild my visible list".
-- ---------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `permission_sync` (
  `user_id` BIGINT NOT NULL,
  `version` BIGINT NOT NULL DEFAULT 0,
  PRIMARY KEY (`user_id`),
  CONSTRAINT `fk_ps_user` FOREIGN KEY (`user_id`)
    REFERENCES `users` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------------
-- audit_log
-- ---------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `audit_log` (
  `id`          BIGINT NOT NULL AUTO_INCREMENT,
  `user_id`     BIGINT NULL,
  `username`    VARCHAR(64)  NOT NULL DEFAULT '',
  `action`      VARCHAR(48)  NOT NULL,
  `target_type` VARCHAR(32)  NOT NULL DEFAULT '',
  `target_id`   BIGINT NULL,
  `detail`      VARCHAR(255) NOT NULL DEFAULT '',
  `created_at`  DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  KEY `ix_audit_user` (`user_id`),
  KEY `ix_audit_created` (`created_at`)
) ENGINE=InnoDB;

-- ---------------------------------------------------------------------------
-- app_meta: key/value for schema_version and the recovery-wrapped DEK
--   recovery_salt        : Argon2id salt for the recovery passphrase
--   recovery_wrapped_dek : AES-256-GCM(KEK_recovery, DEK)
-- ---------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS `app_meta` (
  `meta_key`   VARCHAR(64)   NOT NULL,
  `meta_value` VARBINARY(1024) NOT NULL,
  PRIMARY KEY (`meta_key`)
) ENGINE=InnoDB;
