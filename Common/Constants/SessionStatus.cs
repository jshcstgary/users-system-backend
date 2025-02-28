namespace Common.Constants;

/// <summary>
/// Defines constants that represent the possible states of a user session as strings.
/// </summary>
/// <remarks>
/// These constants should be used whenever representing the status of a user session within the system, indicating whether the session was opened, closed or if there was a failure during the login and logout attempt. Since this is a static class, its members are accessed directly by using the class name (e.g., <c>SessionStatus.Open</c>).
/// </remarks>
public static class SessionStatus
{
	/// <summary>
	/// Represents a closed user session. This indicates that the session has been terminated or the user has logged out.
	/// </summary>
	public const string Close = "CLOSE";

	/// <summary>
	/// Represents a failed user session. This indicates that the session creation or login attempt failed.
	/// </summary>
	public const string Failed = "FAILED";

	/// <summary>
	/// Represents an open user session. This indicates that a session is currently active and the user is logged in.
	/// </summary>
	public const string Open = "OPEN";
}
