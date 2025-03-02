using System.ComponentModel.DataAnnotations;

namespace Common.Models.Dto.RoleOption;

/// <summary>
/// Data Transfer Object (DTO) for creating a new Role Option. This DTO is used when creating a new Role Option record in the database. It contains the necessary information for creating the Role Option, excluding properties that are automatically generated by the database (like Id).
/// </summary>
public class RoleOptionCreateDto
{
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
}
