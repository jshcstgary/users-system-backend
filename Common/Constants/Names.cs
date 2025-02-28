using Common.Models;

namespace Common.Constants;

/// <summary>
/// Defines constants that represent the name of the model entities in the system.
/// </summary>
/// <remarks>
/// These constants should be used whenever representing the name of the models into the system.
/// </remarks>
public static class Names
{
	/// <summary>
	/// Controller class name.
	/// </summary>
	public const string ControllerClass = "Controller class";

	/// <summary>
	/// Service class name.
	/// </summary>
	public const string ServiceClass = "Service class";

	/// <summary>
	/// Repository class name.
	/// </summary>
	public const string RepositoryClass = "Repository class";

	/// <summary>
	/// Create method name.
	/// </summary>
	public const string CreateMethod = "Create";

	/// <summary>
	/// GetAll method name.
	/// </summary>
	public const string GetAllMethod = "GetAll";

	/// <summary>
	/// GetById method name.
	/// </summary>
	public const string GetByIdMethod = "GetById";

	/// <summary>
	/// GetOne method name.
	/// </summary>
	public const string GetOneMethod = "GetOne";

	/// <summary>
	/// Update method name.
	/// </summary>
	public const string UpdateMethod = "Update";

	/// <summary>
	/// Delete method name.
	/// </summary>
	public const string DeleteMethod = "Delete";

	/// <summary>
	/// RoleOption model name.
	/// </summary>
	public const string RoleOptionModel = nameof(RoleOption);

	/// <summary>
	/// Role model name.
	/// </summary>
	public const string RoleModel = nameof(Role);

	/// <summary>
	/// /// Session model name.
	/// </summary>
	public const string SessionModel = nameof(Session);

	/// <summary>
	/// User model name.
	/// </summary>
	public const string UserModel = nameof(User);
}
