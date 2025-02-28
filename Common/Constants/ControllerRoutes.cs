namespace Common.Constants;

/// <summary>
/// Defines constants that represent the paths to access the controllers.
/// </summary>
/// <remarks>
/// These constants should be used when setting the paths to access the controllers.
/// </remarks>
public static class ControllerRoutes
{
	/// <summary>
	/// The prefix to be added to all controller paths.
	/// </summary>
	private const string Prefix = "api";

	/// <summary>
	/// Defines the base path for the role options in the API.
	/// </summary>
	public const string RoleOption = $"{Prefix}/role-option";

	/// <summary>
	/// Defines the base path for the roles in the API.
	/// </summary>
	public const string Role = $"{Prefix}/role";

	/// <summary>
	/// Defines the base path for the users in the API.
	/// </summary>
	public const string User = $"{Prefix}/user";
}
