using System.ComponentModel.DataAnnotations;

using Common.Models.Dto.Role;

namespace Common.Models.Dto.User;

/// <summary>
/// Data Transfer Object (DTO) for creating a new User. This DTO is used when creating a new User record in the database. It contains the necessary information for creating the User, excluding properties that are automatically generated by the database (like Id).
/// </summary>
public class UserCreateDto
{
	/// <summary>
	/// The User's national identification number (DNI). Must contain exactly 10 digits.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	[StringLength(10, MinimumLength = 10, ErrorMessage = "The field {0} must contain exactly {1} digits.")]
	[RegularExpression(@"^[0-9]+$", ErrorMessage = "The field {0} mus contain only numbers.")]
	public string Dni { get; set; } = null!;

	/// <summary>
	/// The User's first name.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	[StringLength(60, MinimumLength = 1, ErrorMessage = "The field {0} must be a maximum of {1} characters.")]
	public string FirstName { get; set; } = null!;

	/// <summary>
	/// The User's last name.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	[StringLength(60, MinimumLength = 1, ErrorMessage = "The field {0} must be a maximum of {1} characters.")]
	public string LastName { get; set; } = null!;

	/// <summary>
	/// The User's birth date.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	public DateOnly BirthDate { get; set; }

	/// <summary>
	/// The User's phone number. Must contain exactly 10 digits and be in a valid format.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	[StringLength(10, MinimumLength = 10, ErrorMessage = "The field {0} must contain exactly {1} digits.")]
	[Phone(ErrorMessage = "The field {0} is not a valid phone number.")]
	public string Phone { get; set; } = null!;

	/// <summary>
	/// The User's username.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", ErrorMessage = "The field {0} is not a valid username.")]
	public string Username { get; set; } = null!;

	/// <summary>
	/// The User's email address.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	[StringLength(60, MinimumLength = 1, ErrorMessage = "The field {0} must be a maximum of {1} characters.")]
	[EmailAddress(ErrorMessage = "The field {0} is not a valid email.")]
	public string Email { get; set; } = null!;

	/// <summary>
	/// The User's password.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	// [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d])[A-Za-z\d[^A-Za-z\d]]{8,16}$", ErrorMessage = "The field {0} is not a valid password.")]
	[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,16}$", ErrorMessage = "The field {0} is not a valid password.")]
	public string Password { get; set; } = null!;

	/// <summary>
	/// The User's role.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	public virtual RoleDto Role { get; set; } = null!;
}
