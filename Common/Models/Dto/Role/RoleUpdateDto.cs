using System.ComponentModel.DataAnnotations;

using Common.Models.Dto.RoleOption;

namespace Common.Models.Dto.Role;

/// <summary>
/// Data Transfer Object (DTO) for updating an existing Role. This DTO is used when updating a Role record in the database. It contains the information that can be modified.
/// </summary>
public class RoleUpdateDto
{
	/// <summary>
	/// The unique identifier for the Role.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	public long Id { get; set; }

	/// <summary>
	/// The name of the Role. This must be between 1 and 30 characters in length.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	[StringLength(30, MinimumLength = 1, ErrorMessage = "The field {0} must contain at least {2} and up to {1} characters.")]
	public string Name { get; set; } = null!;

	/// <summary>
	/// Indicates whether the Role is currently active.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	public bool Status { get; set; }

	/// <summary>
	/// A row version used for optimistic concurrency control.  This ensures data integrity by preventing concurrent updates from overwriting each other.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	public byte[] RowVersion { get; set; } = null!;

	/// <summary>
	/// The date and time when the Role was created.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	public DateTime CreatedAt { get; set; }

	/// <summary>
	/// The date and time when the Role was last updated.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	public DateTime UpdatedAt { get; set; }

	/// <summary>
	/// A collection of RoleOptions associated with this Role.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	public ICollection<RoleOptionDto> RoleOptions { get; set; } = [];
}
