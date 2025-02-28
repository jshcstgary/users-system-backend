using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Common.Constants;
using Common.Models;

using RoleES.Data;
using RoleES.Repository.Interfaces;

namespace RoleES.Repository;

/// <summary>
/// Implementation of <see cref="IRepository"/> that interacts with a data repository for CRUD operations on <see cref="Role"/> entities.
/// </summary>
public class Repository : IRepository
{
	/// <summary>
	/// Constant string representing the base logger for the repository layer.
	/// </summary>
	private const string className = $"{Names.RoleModel}{Names.RepositoryClass}";

	/// <summary>
	/// Logger used to log messages and events.
	/// </summary>
	private readonly ILogger<IRepository> _logger;

	/// <summary>
	/// Application context used to interact with the database.
	/// </summary>
	private readonly AppDbContext _context;

	/// <summary>
	/// Set of <see cref="Role"/> entities in the database.
	/// </summary>
	protected readonly DbSet<Role> dbSet;

	/// <summary>
	/// Initializes a new instance of the <see cref="Repository"/> class.
	/// </summary>
	/// <param name="logger">The logger instance used for logging.</param>
	/// <param name="context">The application context used for interacting with the database.</param>
	public Repository(ILogger<IRepository> logger, AppDbContext context)
	{
		_logger = logger;
		_context = context;
		dbSet = _context.Set<Role>();
	}

	/// <inheritdoc/>
	public async Task Save()
	{
		await _context.SaveChangesAsync();
	}

	/// <inheritdoc/>
	public async Task<Role> Create(Role role)
	{
		string logInfo = $"{className} - {Names.CreateMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			foreach (RoleOption roleOption in role.RoleOptions)
			{
				_context.Set<RoleOption>().Attach(roleOption);
				_context.Entry(roleOption).State = EntityState.Unchanged;
			}

			EntityEntry<Role> entityEntry = await dbSet.AddAsync(role);

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
	public async Task<IEnumerable<Role>> GetAll(int limit, int offset, Expression<Func<Role, bool>>? filter = null)
	{
		string logInfo = $"{className} - {Names.GetAllMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			IQueryable<Role> query = dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			return await query
				.Include(role => role.RoleOptions)
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
	public async Task<Role?> GetOne(Expression<Func<Role, bool>> filter)
	{
		string logInfo = $"{className} - {Names.GetOneMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			return await dbSet
				.Include(role => role.RoleOptions)
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
	public async Task<Role?> Update(Role role)
	{
		string logInfo = $"{className} - {Names.UpdateMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			_context.Set<Role>().Entry(role).State = EntityState.Modified;

			await Save();

			await _context.Entry(role).ReloadAsync();

			return role;
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
	public async Task<bool> Delete(Role role)
	// public async Task<bool> Delete(long id)
	{
		string logInfo = $"{className} - {Names.DeleteMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			_context.Set<Role>().Entry(role).State = EntityState.Modified;

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
