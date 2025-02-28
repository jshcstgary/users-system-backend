using System.ComponentModel.DataAnnotations;

namespace Common.Models.Dto.RoleOption;

/// <summary>
/// Data Transfer Object (DTO) for updating an existing Role Option. This DTO is used when updating a Role Option record in the database. It contains the information that can be modified.
/// </summary>
public class RoleOptionUpdateDto
{
	/// <summary>
	/// The unique identifier for the Role Option.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	public long Id { get; set; }

	/// <summary>
	/// The name of the Role Option. This must be between 1 and 30 characters in length.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	[StringLength(30, MinimumLength = 1, ErrorMessage = "The field {0} must contain at least {2} and up to {1} characters.")]
	public string Name { get; set; } = null!;

	/// <summary>
	/// The link associated with the Role Option. This must be between 1 and 30 characters in length.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	[StringLength(60, MinimumLength = 1, ErrorMessage = "The field {0} must contain at least {2} and up to {1} characters.")]
	public string Link { get; set; } = null!;

	/// <summary>
	/// Indicates whether the Role Option is currently active.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	public bool Status { get; set; }

	/// <summary>
	/// A row version used for optimistic concurrency control. This ensures data integrity by preventing concurrent updates from overwriting each other.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	public byte[] RowVersion { get; set; } = null!;

	/// <summary>
	/// The date and time when the Role Option was created.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	public DateTime CreatedAt { get; set; }

	/// <summary>
	/// The date and time when the Role Option was last updated.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	public DateTime UpdatedAt { get; set; }
}
