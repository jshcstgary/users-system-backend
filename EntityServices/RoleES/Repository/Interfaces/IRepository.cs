using System.Linq.Expressions;

using Common.Models;

namespace RoleES.Repository.Interfaces;

/// <summary>
/// Defines the basic CRUD operations for <see cref="Role"/> entities.
/// </summary>
public interface IRepository
{
	/// <summary>
	/// Saves the changes made in the database context.
	/// </summary>
	/// <returns>A task representing asynchronous operation.</returns>
	public Task Save();

	/// <summary>
	/// Creates a new <see cref="Role"/> entity and adds it to the database context.
	/// </summary>
	/// <param name="role">The <see cref="Role"/> entity to be created.</param>
	/// <returns>A task that represents the asynchronous operation and returns the created <see cref="Role"/> entity.</returns>
	public Task<Role> Create(Role role);

	/// <summary>
	/// Gets a list of <see cref="Role"/> entities from the database.
	/// </summary>
	/// <param name="limit">The maximum number of entities to return.</param>
	/// <param name="offset">The number of entities to omit before starting to return results.</param>
	/// <param name="filter">An optional lambda expression to filter the entities.</param>
	/// <returns>A task that represents the asynchronous operation and returns a collection of <see cref="Role"/> entities.</returns>
	public Task<IEnumerable<Role>> GetAll(int limit, int offset, Expression<Func<Role, bool>>? filter = null);

	/// <summary>
	/// Gets a single <see cref="Role"/> entity from the database that meets the specified filter.
	/// </summary>
	/// <param name="filter">The lambda expression to filter the entity.</param>
	/// <returns>A task that represents the asynchronous operation and returns the <see cref="Role"/> entity, or <c>null</c> if none is found.</returns>
	public Task<Role?> GetOne(Expression<Func<Role, bool>> filter);

	/// <summary>
	/// Updates an existing <see cref="Role"/> entity in the database.
	/// </summary>
	/// <param name="role">The <see cref="Role"/> entity to update.</param>
	/// <returns>A task that represents the asynchronous operation and returns the updated <see cref="Role"/> entity.</returns>
	public Task<Role?> Update(Role role);

	/// <summary>
	/// Logically removes a <see cref="Role"/> entity from the database.
	/// </summary>
	/// <param name="role">The <see cref="Role"/> entity to be eliminated.</param>
	/// <returns>A task that represents the asynchronous operation and returns true if the deletion was successful, false otherwise.</returns>
	public Task<bool> Delete(Role role);
}
