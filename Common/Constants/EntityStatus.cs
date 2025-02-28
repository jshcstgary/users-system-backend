namespace Common.Constants;

/// <summary>
/// Defines constants that represent the active and inactive states of entities within
/// the application.
/// </summary>
/// <remarks>
/// These constants should be used whenever representing the active/inactive status
/// of any entity within the system.
/// </remarks>
public class EntityStatus
{
	/// <summary>
	/// Represents the active status of an entity. This indicates that the entity is currently operational and available for use. It is equivalent to the boolean value <c>true</c>.
	/// </summary>
	public const bool Active = true;

	/// <summary>
	/// Represents the inactive status of an entity. This indicates that the entity is currently not operational or unavailable for use. It is equivalent to the boolean value <c>false</c>.
	/// </summary>
	public const bool Inactive = false;
}
