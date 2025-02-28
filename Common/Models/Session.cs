namespace Common.Models;

/// <summary>
/// Table to store de session of the users.
/// </summary>
public class Session
{
	/// <summary>
	/// The unique identifier for the session.
	/// </summary>
	public long Id { get; set; }

	/// <summary>
	/// The date and time when the user signed in. This can be null if the session hasn't started yet.
	/// </summary>
	public DateTime? SignInDate { get; set; }

	/// <summary>
	/// The date and time when the user signed out. This can be null if the session is still active.
	/// </summary>
	public DateTime? SignOutDate { get; set; }

	/// <summary>
	/// The status of the session. This could be a string representing values like "OPEN", "FAILED", or "CLOSE".
	/// </summary>
	public string Status { get; set; } = null!;

	/// <summary>
	/// The reason for the session ending (e.g., sign-out, timeout, etc.). This can be null if the session is active or ended normally.
	/// </summary>
	public string? Reason { get; set; }

	/// <summary>
	/// The ID of the user associated with this session.
	/// </summary>
	public long UserId { get; set; }

	/// <summary>
	/// The user associated with this session.
	/// </summary>
	public virtual User User { get; set; } = null!;
}
