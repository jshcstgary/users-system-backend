namespace Common.Constants;

/// <summary>
/// Defines constants representing the active status of a user session.
/// </summary>
/// <remarks>
/// These constants should be used whenever representing the active/inactive status of a user session within the system. Since this is a static class, its members are accessed directly using the class name (e.g., <c>SessionActive.Yes</c>).
/// </remarks>
public static class SessionActive
{
	/// <summary>
	/// Represents an inactive user session. This indicates that the user is not currently logged in or their session has expired or been terminated. It is equivalent to the boolean value <c>false</c>.
	/// </summary>
	public const bool No = false;

	/// <summary>
	/// Represents an active user session. This indicates that the user is currently logged in and their session is active. It is equivalent to the boolean value <c>true</c>.
	/// </summary>
	public const bool Yes = true;
}
