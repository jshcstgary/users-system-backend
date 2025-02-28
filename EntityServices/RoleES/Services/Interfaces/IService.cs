using System.Linq.Expressions;

using Common.Models;
using Common.Models.Dto.Role;

namespace RoleES.Services.Interfaces;

/// <summary>
/// Defines the service CRUD operations for the management of <see cref="Role"/>.
/// </summary>
public interface IService
{
	/// <summary>
	/// Creates a new <see cref="Role"/>.
	/// </summary>
	/// <param name="roleCreateDto">The data for the creation of the <see cref="Role"/>.</param>
	/// <returns>A task that represents the asynchronous operation and returns the DTO of the created <see cref="Role"/>.</returns>
	public Task<RoleDto> Create(RoleCreateDto roleCreateDto);

	/// <summary>
	/// Gets a list of <see cref="Role"/>.
	/// </summary>
	/// <param name="limit">The maximum number of <see cref="Role"/> to return.</param>
	/// <param name="offset">The number of <see cref="Role"/> to skip before starting to return results.</param>
	/// <param name="filter">An optional lambda expression to filter the <see cref="Role"/>.</param>
	/// <returns>A task that represents the asynchronous operation and returns a collection of DTOs of <see cref="Role"/>.</returns>
	public Task<IEnumerable<RoleDto>> GetAll(int limit, int offset, Expression<Func<Role, bool>>? filter = null);

	/// <summary>
	/// Gets a specific <see cref="Role"/>.
	/// </summary>
	/// <param name="filter">The lambda expression to filter the <see cref="Role"/>.</param>
	/// <returns>A task that represents the asynchronous operation and returns the DTO of the <see cref="Role"/>, or null if not found.</returns>
	public Task<RoleDto?> GetOne(Expression<Func<Role, bool>> filter);

	/// <summary>
	/// Updates an existing <see cref="Role"/>.
	/// </summary>
	/// <param name="roleUpdateDto">The data for the update of the <see cref="Role"/>.</param>
	/// <returns>A task that represents the asynchronous operation and returns the DTO of the updated <see cref="Role"/>, or null if not found.</returns>
	public Task<RoleDto?> Update(RoleUpdateDto roleUpdateDto);

	/// <summary>
	/// Deletes a <see cref="Role"/> by its ID.
	/// </summary>
	/// <param name="id">The ID of the <see cref="Role"/> to be deleted.</param>
	/// <returns>A task that represents the asynchronous operation and returns true if the deletion was successful, false otherwise.</returns>
	public Task<bool> Delete(int id);
}
