using System.ComponentModel.DataAnnotations;

using Common.Models.Dto.Role;

namespace Common.Models.Dto.User;

/// <summary>
/// Data Transfer Object (DTO) representing a User. This DTO is used for transferring User data between layers of the application, such as between the API and the business logic.
/// </summary>
public class UserDto
{
	/// <summary>
	/// The unique identifier for the User.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	public long Id { get; set; }

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
	/// Indicates whether the User has an active session.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	public bool ActiveSession { get; set; }

	/// <summary>
	/// Indicates whether the User account is active.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	public bool Status { get; set; }

	/// <summary>
	/// The date and time when the User account was created.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	public DateTime CreatedAt { get; set; }

	/// <summary>
	/// The date and time when the User account was last updated.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	public DateTime UpdatedAt { get; set; }

	/// <summary>
	/// A row version used for optimistic concurrency control. This ensures data integrity by preventing concurrent updates from overwriting each other.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	public byte[] RowVersion { get; set; } = null!;

	/// <summary>
	/// The User's role.
	/// </summary>
	[Required(ErrorMessage = "The field {0} is required")]
	public virtual RoleDto Role { get; set; } = null!;
}
