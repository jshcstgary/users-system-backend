namespace Common.Constants;

/// <summary>
/// Defines constants that represent messages to throw new exceptions.
/// </summary>
/// <remarks>
/// These constants should be used when you need to throw an exception with a specific message so that it can be identified later.
/// </remarks>
public static class ExceptionMessages
{
	/// <summary>
	/// Message to identify exceptions related to the deletion or deleted status of a record.
	/// </summary>
	public const string AlreadyDeleted = "DELETED";

	/// <summary>
	/// Message to identify exceptions related to the nullity of a record.
	/// </summary>
	public const string Null = "NULL";
}
