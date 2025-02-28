using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Common.Constants;
using Common.Models;

using UserES.Data;
using UserES.Repository.Interfaces;

namespace UserES.Repository;

/// <summary>
/// Implementation of <see cref="IRepository"/> that interacts with a data repository for CRUD operations on <see cref="User"/> entities.
/// </summary>
public class Repository : IRepository
{
	/// <summary>
	/// Constant string representing the base logger for the repository layer.
	/// </summary>
	private const string className = $"{Names.UserModel}{Names.RepositoryClass}";

	/// <summary>
	/// Logger used to log messages and events.
	/// </summary>
	private readonly ILogger<IRepository> _logger;

	/// <summary>
	/// Application context used to interact with the database.
	/// </summary>
	private readonly AppDbContext _context;

	/// <summary>
	/// Set of <see cref="User"/> entities in the database.
	/// </summary>
	protected readonly DbSet<User> dbSet;

	/// <summary>
	/// Initializes a new instance of the <see cref="Repository"/> class.
	/// </summary>
	/// <param name="logger">The logger instance used for logging.</param>
	/// <param name="context">The application context used for interacting with the database.</param>
	public Repository(ILogger<IRepository> logger, AppDbContext context)
	{
		_logger = logger;
		_context = context;
		dbSet = _context.Set<User>();
	}

	/// <inheritdoc/>
	public async Task Save()
	{
		await _context.SaveChangesAsync();
	}

	/// <inheritdoc/>
	public async Task<User> Create(User user)
	{
		string logInfo = $"{className} - {Names.CreateMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			_context.Set<Role>().Attach(user.Role);
			_context.Entry(user.Role).State = EntityState.Unchanged;

			foreach (RoleOption roleOption in user.Role.RoleOptions)
			{
				_context.Set<RoleOption>().Attach(roleOption);
				_context.Entry(roleOption).State = EntityState.Unchanged;
			}

			EntityEntry<User> entityEntry = await dbSet.AddAsync(user);

			await Save();

			return entityEntry.Entity;
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			_logger.LogInformation($"Leaving {logInfo}");
		}
	}

	/// <inheritdoc/>
	public async Task<IEnumerable<User>> GetAll(int limit, int offset, Expression<Func<User, bool>>? filter = null)
	{
		string logInfo = $"{className} - {Names.GetAllMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			IQueryable<User> query = dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			return await query
				.Include(user => user.Role)
					.ThenInclude(role => role.RoleOptions)
				.Skip(offset)
				.Take(limit)
				.AsNoTracking()
				.ToListAsync();
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			_logger.LogInformation($"Leaving {logInfo}");
		}
	}

	/// <inheritdoc/>
	public async Task<User?> GetOne(Expression<Func<User, bool>> filter)
	{
		string logInfo = $"{className} - {Names.GetOneMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			return await dbSet
				.Include(user => user.Role)
					.ThenInclude(role => role.RoleOptions)
				.AsNoTracking()
				.FirstOrDefaultAsync(filter);
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			_logger.LogInformation($"Leaving {logInfo}");
		}
	}

	/// <inheritdoc/>
	public async Task<User> Update(User user)
	{
		string logInfo = $"{className} - {Names.UpdateMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			_context.Set<User>().Entry(user).State = EntityState.Modified;

			await Save();

			await _context.Entry(user).ReloadAsync();

			return user;
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			_logger.LogInformation($"Leaving {logInfo}");
		}
	}

	/// <inheritdoc/>
	public async Task<bool> Delete(User user)
	{
		string logInfo = $"{className} - {Names.DeleteMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			_context.Set<User>().Entry(user).State = EntityState.Modified;

			await Save();

			return true;
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			_logger.LogInformation($"Leaving {logInfo}");
		}
	}
}
