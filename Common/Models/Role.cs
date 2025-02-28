using Common.Constants;

namespace Common.Models;

/// <summary>
/// Table to store the roles.
/// </summary>
public class Role
{
	/// <summary>
	/// The unique identifier for the role.
	/// </summary>
	public long Id { get; set; }

	/// <summary>
	/// The name of the role. This should be a descriptive and unique value.
	/// </summary>
	public string Name { get; set; } = null!;

	/// <summary>
	/// Indicates whether the role is currently active. True if role is active and can be assigned to users, false otherwise.
	/// </summary>
	public bool Status { get; set; } = EntityStatus.Active;

	/// <summary>
	/// A row version used for optimistic concurrency control. This property is automatically managed by the database.
	/// </summary>
	public byte[] RowVersion { get; set; } = null!;

	/// <summary>
	/// The date and time when the role was created.
	/// </summary>
	public DateTime CreatedAt { get; set; }

	/// <summary>
	/// The date and time when the role was last updated.
	/// </summary>
	public DateTime UpdatedAt { get; set; }

	/// <summary>
	/// A collection of users that belong to this role.
	/// </summary>
	public virtual ICollection<User> Users { get; set; } = [];

	/// <summary>
	/// A collection of role options associated with this role. These options define the specific permissions or functionalities granted to the role.
	/// </summary>
	public virtual ICollection<RoleOption> RoleOptions { get; set; } = [];
}
