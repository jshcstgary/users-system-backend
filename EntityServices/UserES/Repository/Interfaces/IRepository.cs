using System.Linq.Expressions;

using Common.Models;

namespace UserES.Repository.Interfaces;

/// <summary>
/// Defines the basic CRUD operations for <see cref="User"/> entities.
/// </summary>
public interface IRepository
{
	/// <summary>
	/// Saves the changes made in the database context.
	/// </summary>
	/// <returns>A task representing asynchronous operation.</returns>
	public Task Save();

	/// <summary>
	/// Creates a new <see cref="User"/> entity and adds it to the database context.
	/// </summary>
	/// <param name="user">The <see cref="User"/> entity to be created.</param>
	/// <returns>A task that represents the asynchronous operation and returns the created <see cref="User"/> entity.</returns>
	public Task<User> Create(User user);

	/// <summary>
	/// Gets a list of <see cref="User"/> entities from the database.
	/// </summary>
	/// <param name="limit">The maximum number of entities to return.</param>
	/// <param name="offset">The number of entities to omit before starting to return results.</param>
	/// <param name="filter">An optional lambda expression to filter the entities.</param>
	/// <returns>A task that represents the asynchronous operation and returns a collection of <see cref="User"/> entities.</returns>
	public Task<IEnumerable<User>> GetAll(int limit, int offset, Expression<Func<User, bool>>? filter = null);

	/// <summary>
	/// Gets a single <see cref="User"/> entity from the database that meets the specified filter.
	/// </summary>
	/// <param name="filter">The lambda expression to filter the entity.</param>
	/// <returns>A task that represents the asynchronous operation and returns the <see cref="User"/> entity, or <c>null</c> if none is found.</returns>
	public Task<User?> GetOne(Expression<Func<User, bool>> filter);

	/// <summary>
	/// Updates an existing <see cref="User"/> entity in the database.
	/// </summary>
	/// <param name="user">The <see cref="User"/> entity to update.</param>
	/// <returns>A task that represents the asynchronous operation and returns the updated <see cref="User"/> entity.</returns>
	public Task<User> Update(User user);

	/// <summary>
	/// Logically removes a <see cref="User"/> entity from the database.
	/// </summary>
	/// <param name="user">The <see cref="User"/> entity to be eliminated.</param>
	/// <returns>A task that represents the asynchronous operation and returns true if the deletion was successful, false otherwise.</returns>
	public Task<bool> Delete(User user);
}
