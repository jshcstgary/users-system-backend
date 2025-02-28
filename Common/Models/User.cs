using Common.Constants;

namespace Common.Models;

/// <summary>
/// Table to store persons.
/// </summary>
public class User
{
	/// <summary>
	/// The unique identifier for the user.
	/// </summary>
	public long Id { get; set; }

	/// <summary>
	/// The user's national identification document number (DNI).
	/// </summary>
	public string Dni { get; set; } = null!;

	/// <summary>
	/// The user's first name.
	/// </summary>
	public string FirstName { get; set; } = null!;

	/// <summary>
	/// The user's last name.
	/// </summary>
	public string LastName { get; set; } = null!;

	/// <summary>
	/// The user's birth date.
	/// </summary>
	public DateOnly BirthDate { get; set; }

	/// <summary>
	/// The user's phone number.
	/// </summary>
	public string Phone { get; set; } = null!;

	/// <summary>
	/// The user's username.
	/// </summary>
	public string Username { get; set; } = null!;

	/// <summary>
	/// The user's email address.
	/// </summary>
	public string Email { get; set; } = null!;

	/// <summary>
	/// The user's password (should be securely hashed).
	/// </summary>
	public string Password { get; set; } = null!;

	/// <summary>
	/// Indicates whether the user has an active session.
	/// </summary>
	public bool ActiveSession { get; set; }

	/// <summary>
	/// Indicates whether the user account is active.
	/// </summary>
	public bool Status { get; set; } = EntityStatus.Active;

	/// <summary>
	/// The date and time when the user account was created.
	/// </summary>
	public DateTime CreatedAt { get; set; }

	/// <summary>
	/// The date and time when the user account was last updated.
	/// </summary>
	public DateTime UpdatedAt { get; set; }

	/// <summary>
	/// A row version used for optimistic concurrency control. This property is automatically managed by the database.
	/// </summary>
	public byte[] RowVersion { get; set; } = null!;

	/// <summary>
	/// The ID of the role assigned to the user.
	/// </summary>
	public long RoleId { get; set; }

	/// <summary>
	/// The role assigned to the user.
	/// </summary>
	public virtual Role Role { get; set; } = null!;

	/// <summary>
	/// A collection of sessions associated with this user.
	/// </summary>
	public virtual ICollection<Session> Sessions { get; set; } = [];
}
