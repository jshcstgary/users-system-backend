namespace Common.Constants;

/// <summary>
/// Defines constants that represent the success or failed status of an HTTP request.
/// </summary>
/// <remarks>
/// These constants should be used whenever representing the success/failure status of an HTTP request.
/// </remarks>
public static class ApiStatus
{
	/// <summary>
	/// Indicates that the HTTP request was successful.
	/// </summary>
	public const bool Success = true;

	/// <summary>
	/// Indicates that the HTTP request failed.
	/// </summary>
	public const bool Failed = false;
}
