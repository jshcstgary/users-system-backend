using System.Linq.Expressions;

using Common.Models;
using Common.Models.Dto.RoleOption;

namespace RoleOptionES.Services.Interfaces;

/// <summary>
/// Defines the service CRUD operations for the management of <see cref="RoleOption"/>.
/// </summary>
public interface IService
{
	/// <summary>
	/// Creates a new <see cref="RoleOption"/>.
	/// </summary>
	/// <param name="roleOptionCreateDto">The data for the creation of the <see cref="RoleOption"/>.</param>
	/// <returns>A task that represents the asynchronous operation and returns the DTO of the created <see cref="RoleOption"/>.</returns>
	public Task<RoleOptionDto> Create(RoleOptionCreateDto roleOptionCreateDto);

	/// <summary>
	/// Gets a list of <see cref="RoleOption"/>.
	/// </summary>
	/// <param name="limit">The maximum number of <see cref="RoleOption"/> to return.</param>
	/// <param name="offset">The number of <see cref="RoleOption"/> to skip before starting to return results.</param>
	/// <param name="filter">An optional lambda expression to filter the <see cref="RoleOption"/>.</param>
	/// <returns>A task that represents the asynchronous operation and returns a collection of DTOs of <see cref="RoleOption"/>.</returns>
	public Task<IEnumerable<RoleOptionDto>> GetAll(int limit, int offset, Expression<Func<RoleOption, bool>>? filter = null);

	/// <summary>
	/// Gets a specific <see cref="RoleOption"/>.
	/// </summary>
	/// <param name="filter">The lambda expression to filter the <see cref="RoleOption"/>.</param>
	/// <returns>A task that represents the asynchronous operation and returns the DTO of the <see cref="RoleOption"/>, or null if not found.</returns>
	public Task<RoleOptionDto?> GetOne(Expression<Func<RoleOption, bool>> filter);

	/// <summary>
	/// Updates an existing <see cref="RoleOption"/>.
	/// </summary>
	/// <param name="roleOptionUpdateDto">The data for the update of the <see cref="RoleOption"/>.</param>
	/// <returns>A task that represents the asynchronous operation and returns the DTO of the updated <see cref="RoleOption"/>, or null if not found.</returns>
	public Task<RoleOptionDto?> Update(RoleOptionUpdateDto roleOptionUpdateDto);

	/// <summary>
	/// Deletes a <see cref="RoleOption"/> by its ID.
	/// </summary>
	/// <param name="id">The ID of the <see cref="RoleOption"/> to be deleted.</param>
	/// <returns>A task that represents the asynchronous operation and returns true if the deletion was successful, false otherwise.</returns>
	public Task<bool> Delete(int id);
}
