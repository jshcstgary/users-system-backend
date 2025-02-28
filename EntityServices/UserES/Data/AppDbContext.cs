using Microsoft.EntityFrameworkCore;

using Common.Models;

namespace UserES.Data;

public partial class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	public virtual DbSet<Role> Roles { get; set; }

	public virtual DbSet<RoleOption> RoleOptions { get; set; }

	public virtual DbSet<User> Users { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Role>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__ROLES");

			entity.ToTable("ROLES", "maintainer", tb => tb.HasComment("Table to store the roles."));

			entity.HasIndex(e => e.Name, "IX__UQ__ROLES__NAME").IsUnique();

			entity.Property(e => e.Id)
				.HasComment("The unique identifier for the role.")
				.HasColumnName("id");
			entity.Property(e => e.CreatedAt)
				.HasDefaultValueSql("(sysutcdatetime())")
				.HasComment("The date and time when the role was created.")
				.HasColumnName("created_at");
			entity.Property(e => e.Name)
				.HasMaxLength(30)
				.HasComment("The name of the role. This should be a descriptive and unique value.")
				.HasColumnName("name");
			entity.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasComment("A row version used for optimistic concurrency control. This property is automatically managed by the database.")
				.HasColumnName("row_version");
			entity.Property(e => e.Status)
				.HasDefaultValue(true)
				.HasComment("Indicates whether the role is currently active. True if role is active and can be assigned to users, false otherwise.")
				.HasColumnName("status");
			entity.Property(e => e.UpdatedAt)
				.HasDefaultValueSql("(sysutcdatetime())")
				.HasComment("The date and time when the role was last updated.")
				.HasColumnName("updated_at");

			entity.HasMany(d => d.RoleOptions).WithMany(p => p.Roles)
				.UsingEntity<Dictionary<string, object>>(
					"RolesRoleOption",
					r => r.HasOne<RoleOption>().WithMany()
						.HasForeignKey("RoleOptionId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK__ROLES__ROLE_OPTIONS__ROLE_OPTIONS"),
					l => l.HasOne<Role>().WithMany()
						.HasForeignKey("RoleId")
						.OnDelete(DeleteBehavior.ClientSetNull)
						.HasConstraintName("FK__ROLES__ROLE_OPTIONS__ROLES"),
					j =>
					{
						j.HasKey("RoleId", "RoleOptionId").HasName("PK__ROLES__ROLE_OPTIONS");
						j.ToTable("ROLES_ROLE_OPTIONS", "maintainer", tb => tb.HasComment("Table for relating roles to role options."));
						j.IndexerProperty<long>("RoleId").HasColumnName("role_id");
						j.IndexerProperty<long>("RoleOptionId").HasColumnName("role_option_id");
					});
		});

		modelBuilder.Entity<RoleOption>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__ROLE_OPTIONS");

			entity.ToTable("ROLE_OPTIONS", "maintainer", tb => tb.HasComment("Table to store the options of the roles."));

			entity.HasIndex(e => e.Link, "IX__UQ__ROLE_OPTIONS__LINK").IsUnique();

			entity.HasIndex(e => e.Name, "IX__UQ__ROLE_OPTIONS__NAME").IsUnique();

			entity.Property(e => e.Id)
				.HasComment("The unique identifier for the role option.")
				.HasColumnName("id");
			entity.Property(e => e.CreatedAt)
				.HasDefaultValueSql("(sysutcdatetime())")
				.HasComment("The date and time when the role option was created.")
				.HasColumnName("created_at");
			entity.Property(e => e.Link)
				.HasMaxLength(60)
				.HasComment("The link or path associated with this role option. This could be a full URL starting with '/' that corresponds to a specific functionality or page within the application.")
				.HasColumnName("link");
			entity.Property(e => e.Name)
				.HasMaxLength(30)
				.HasComment("The name of the role option. This should be a descriptive and unique value. Examples might include \"Users\", \"Roles\", etc.")
				.HasColumnName("name");
			entity.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasComment("A row version used for optimistic concurrency control. This property is automatically managed by the database.")
				.HasColumnName("row_version");
			entity.Property(e => e.Status)
				.HasComment("Indicates whether the role option is currently active. True if role option is active and can be assigned to roles, false otherwise.")
				.HasColumnName("status");
			entity.Property(e => e.UpdatedAt)
				.HasDefaultValueSql("(sysutcdatetime())")
				.HasComment("The date and time when the role option was last updated.")
				.HasColumnName("updated_at");
		});

		modelBuilder.Entity<User>(entity =>
		{
			entity.HasKey(e => e.Id).HasName("PK__USERS");

			entity.ToTable("USERS", "user", tb => tb.HasComment("Table to store persons."));

			entity.HasIndex(e => e.Dni, "IX__UQ__USERS__DNI").IsUnique();

			entity.HasIndex(e => e.Email, "IX__UQ__USERS__EMAIL").IsUnique();

			entity.HasIndex(e => e.Phone, "IX__UQ__USERS__PHONE").IsUnique();

			entity.HasIndex(e => e.Username, "IX__UQ__USERS__USERNAME").IsUnique();

			entity.Property(e => e.Id)
				.HasComment("The unique identifier for the user.")
				.HasColumnName("id");
			entity.Property(e => e.ActiveSession)
				.HasComment("Indicates whether the user has an active session.")
				.HasColumnName("active_session");
			entity.Property(e => e.BirthDate)
				.HasComment("The user's birth date.")
				.HasColumnName("birth_date");
			entity.Property(e => e.CreatedAt)
				.HasDefaultValueSql("(sysutcdatetime())")
				.HasComment("The date and time when the user account was created.")
				.HasColumnName("created_at");
			entity.Property(e => e.Dni)
				.HasMaxLength(10)
				.IsUnicode(false)
				.IsFixedLength()
				.HasComment("The user's national identification document number (DNI).")
				.HasColumnName("dni");
			entity.Property(e => e.Email)
				.HasMaxLength(60)
				.HasComment("The user's email address.")
				.HasColumnName("email");
			entity.Property(e => e.FirstName)
				.HasMaxLength(60)
				.HasComment("The user's first name.")
				.HasColumnName("first_name");
			entity.Property(e => e.LastName)
				.HasMaxLength(60)
				.HasComment("The user's last name.")
				.HasColumnName("last_name");
			entity.Property(e => e.Password)
				.HasComment("The user's password (should be securely hashed).")
				.HasColumnType("text")
				.HasColumnName("password");
			entity.Property(e => e.Phone)
				.HasMaxLength(10)
				.IsUnicode(false)
				.IsFixedLength()
				.HasComment("The user's phone number.")
				.HasColumnName("phone");
			entity.Property(e => e.RoleId)
				.HasComment("The ID of the role assigned to the user.")
				.HasColumnName("role_id");
			entity.Property(e => e.RowVersion)
				.IsRowVersion()
				.IsConcurrencyToken()
				.HasComment("A row version used for optimistic concurrency control. This property is automatically managed by the database.")
				.HasColumnName("row_version");
			entity.Property(e => e.Status)
				.HasDefaultValue(true)
				.HasComment("Indicates whether the user account is active.")
				.HasColumnName("status");
			entity.Property(e => e.UpdatedAt)
				.HasDefaultValueSql("(sysutcdatetime())")
				.HasComment("The date and time when the user account was last updated.")
				.HasColumnName("updated_at");
			entity.Property(e => e.Username)
				.HasMaxLength(60)
				.HasComment("The user's username.")
				.HasColumnName("username");

			entity.HasOne(d => d.Role).WithMany(p => p.Users)
				.HasForeignKey(d => d.RoleId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("FK__USERS__ROLES");
		});

		OnModelCreatingPartial(modelBuilder);
	}

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
