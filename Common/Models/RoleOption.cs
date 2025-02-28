using Common.Constants;

namespace Common.Models;

/// <summary>
/// Table to store the options of the roles.
/// </summary>
public class RoleOption
{
	/// <summary>
	/// The unique identifier for the role option.
	/// </summary>
	public long Id { get; set; }

	/// <summary>
	/// The name of the role option. This should be a descriptive and unique value. Examples might include "Users", "Roles", etc.
	/// </summary>
	public string Name { get; set; } = null!;

	/// <summary>
	/// The link or path associated with this role option. This could be a full URL starting with '/' that corresponds to a specific functionality or page within the application.
	/// </summary>
	public string Link { get; set; } = null!;

	/// <summary>
	/// Indicates whether the role option is currently active. True if role option is active and can be assigned to roles, false otherwise.
	/// </summary>
	public bool Status { get; set; } = EntityStatus.Active;

	/// <summary>
	/// A row version used for optimistic concurrency control. This property is automatically managed by the database.
	/// </summary>
	public byte[] RowVersion { get; set; } = null!;

	/// <summary>
	/// The date and time when the role option was created.
	/// </summary>
	public DateTime CreatedAt { get; set; }

	/// <summary>
	/// The date and time when the role option was last updated.
	/// </summary>
	public DateTime UpdatedAt { get; set; }

	/// <summary>
	/// A collection of roles that have this option assigned.
	/// </summary>
	public virtual ICollection<Role> Roles { get; set; } = [];
}
