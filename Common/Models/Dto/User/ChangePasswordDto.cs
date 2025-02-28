using System.ComponentModel.DataAnnotations;

namespace Common.Models.Dto.User;

/// <summary>
/// Data Transfer Object (DTO) to change a user's password. This DTO is used when a user's password needs to be changed. It contains the information needed to change a user's password, excluding properties that are not needed for this scenario.
/// </summary>
public class ChangePasswordDto
{
	/// <summary>
	/// The unique identifier for the User.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	public long Id { get; set; }

	/// <summary>
	/// The current User's password.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,16}$", ErrorMessage = "The field {0} is not a valid password.")]
	public string CurrentPassword { get; set; } = null!;

	/// <summary>
	/// The new User's password.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,16}$", ErrorMessage = "The field {0} is not a valid password.")]
	public string NewPassword { get; set; } = null!;
}
