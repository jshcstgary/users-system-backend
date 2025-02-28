using System.Linq.Expressions;

using Common.Models;

namespace RoleOptionES.Repository.Interfaces;

/// <summary>
/// Defines the basic CRUD operations for <see cref="RoleOption"/> entities.
/// </summary>
public interface IRepository
{
	/// <summary>
	/// Saves the changes made in the database context.
	/// </summary>
	/// <returns>A task representing asynchronous operation.</returns>
	public Task Save();

	/// <summary>
	/// Creates a new <see cref="RoleOption"/> entity and adds it to the database context.
	/// </summary>
	/// <param name="roleOption">The <see cref="RoleOption"/> entity to be created.</param>
	/// <returns>A task that represents the asynchronous operation and returns the created <see cref="RoleOption"/> entity.</returns>
	public Task<RoleOption> Create(RoleOption roleOption);

	/// <summary>
	/// Gets a list of <see cref="RoleOption"/> entities from the database.
	/// </summary>
	/// <param name="limit">The maximum number of entities to return.</param>
	/// <param name="offset">The number of entities to omit before starting to return results.</param>
	/// <param name="filter">An optional lambda expression to filter the entities.</param>
	/// <returns>A task that represents the asynchronous operation and returns a  collection of <see cref="RoleOption"/> entities.</returns>
	public Task<IEnumerable<RoleOption>> GetAll(int limit, int offset, Expression<Func<RoleOption, bool>>? filter = null);

	/// <summary>
	/// Gets a single <see cref="RoleOption"/> entity from the database that meets the specified filter.
	/// </summary>
	/// <param name="filter">The lambda expression to filter the entity.</param>
	/// <returns>A task that represents the asynchronous operation and returns the <see cref="RoleOption"/> entity, or <c>null</c> if none is found.</returns>
	public Task<RoleOption?> GetOne(Expression<Func<RoleOption, bool>> filter);

	/// <summary>
	/// Updates an existing <see cref="RoleOption"/> entity in the database.
	/// </summary>
	/// <param name="roleOption">The <see cref="RoleOption"/> entity to update.</param>
	/// <returns>A task that represents the asynchronous operation and returns the  updated <see cref="RoleOption"/> entity.</returns>
	public Task<RoleOption> Update(RoleOption roleOption);

	/// <summary>
	/// Logically removes a <see cref="RoleOption"/> entity from the database.
	/// </summary>
	/// <param name="roleOption">The <see cref="RoleOption"/> entity to be eliminated.</param>
	/// <returns>A task that represents the asynchronous operation and returns true if the deletion was successful, false otherwise.</returns>
	public Task<bool> Delete(RoleOption roleOption);
}
