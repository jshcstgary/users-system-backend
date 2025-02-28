using System.Linq.Expressions;

using Common.Models;
using Common.Models.Dto.User;

namespace UserES.Services.Interfaces;

/// <summary>
/// Defines the service CRUD operations for the management of <see cref="User"/>.
/// </summary>
public interface IService
{
	/// <summary>
	/// Creates a new <see cref="User"/>.
	/// </summary>
	/// <param name="userCreateDto">The data for the creation of the <see cref="User"/>.</param>
	/// <returns>A task that represents the asynchronous operation and returns the DTO of the created <see cref="User"/>.</returns>
	public Task<UserDto> Create(UserCreateDto userCreateDto);

	/// <summary>
	/// Gets a list of <see cref="User"/>.
	/// </summary>
	/// <param name="limit">The maximum number of <see cref="User"/> to return.</param>
	/// <param name="offset">The number of <see cref="User"/> to skip before starting to return results.</param>
	/// <param name="filter">An optional lambda expression to filter the <see cref="User"/>.</param>
	/// <returns>A task that represents the asynchronous operation and returns a collection of DTOs of <see cref="User"/>.</returns>
	public Task<IEnumerable<UserDto>> GetAll(int limit, int offset, Expression<Func<User, bool>>? filter = null);

	/// <summary>
	/// Gets a specific <see cref="User"/>.
	/// </summary>
	/// <param name="filter">The lambda expression to filter the <see cref="User"/>.</param>
	/// <returns>A task that represents the asynchronous operation and returns the DTO of the <see cref="User"/>, or null if not found.</returns>
	public Task<UserDto?> GetOne(Expression<Func<User, bool>> filter);

	/// <summary>
	/// Updates an existing <see cref="User"/>.
	/// </summary>
	/// <param name="userUpdateDto">The data for the update of the <see cref="User"/>.</param>
	/// <returns>A task that represents the asynchronous operation and returns the DTO of the updated <see cref="User"/>, or null if not found.</returns>
	public Task<UserDto?> Update(UserUpdateDto userUpdateDto);

	/// <summary>
	/// Deletes a <see cref="User"/> by its ID.
	/// </summary>
	/// <param name="id">The ID of the <see cref="User"/> to be deleted.</param>
	/// <returns>A task that represents the asynchronous operation and returns true if the deletion was successful, false otherwise.</returns>
	public Task<bool> Delete(int id);
}
