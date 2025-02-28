using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Common.Constants;
using Common.Models;

using RoleOptionES.Data;
using RoleOptionES.Repository.Interfaces;

namespace RoleOptionES.Repository;

/// <summary>
/// Implementation of <see cref="IRepository"/> that interacts with a data repository for CRUD operations on <see cref="RoleOption"/> entities.
/// </summary>
public class Repository : IRepository
{
	/// <summary>
	/// Constant string representing the base logger for the repository layer.
	/// </summary>
	private const string className = $"{Names.RoleOptionModel}{Names.RepositoryClass}";

	/// <summary>
	/// Logger used to log messages and events.
	/// </summary>
	private readonly ILogger<IRepository> _logger;

	/// <summary>
	/// Application context used to interact with the database.
	/// </summary>
	private readonly AppDbContext _context;

	/// <summary>
	/// Set of <see cref="RoleOption"/> entities in the database.
	/// </summary>
	protected readonly DbSet<RoleOption> dbSet;

	/// <summary>
	/// Initializes a new instance of the <see cref="Repository"/> class.
	/// </summary>
	/// <param name="logger">The logger instance used for logging.</param>
	/// <param name="context">The application context used for interacting with the database.</param>
	public Repository(ILogger<IRepository> logger, AppDbContext context)
	{
		_logger = logger;
		_context = context;
		dbSet = _context.Set<RoleOption>();
	}

	/// <inheritdoc/>
	public async Task Save()
	{
		await _context.SaveChangesAsync();
	}

	/// <inheritdoc/>
	public async Task<RoleOption> Create(RoleOption roleOption)
	{
		string logInfo = $"{className} - {Names.CreateMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			EntityEntry<RoleOption> entityEntry = await dbSet.AddAsync(roleOption);

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
	public async Task<IEnumerable<RoleOption>> GetAll(int limit, int offset, Expression<Func<RoleOption, bool>>? filter = null)
	{
		string logInfo = $"{className} - {Names.GetAllMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			IQueryable<RoleOption> query = dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			return await query
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
	public async Task<RoleOption?> GetOne(Expression<Func<RoleOption, bool>> filter)
	{
		string logInfo = $"{className} - {Names.GetOneMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			return await dbSet
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
	public async Task<RoleOption> Update(RoleOption roleOption)
	{
		string logInfo = $"{className} - {Names.UpdateMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			EntityEntry<RoleOption> entityEntry = dbSet.Update(roleOption);

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
	public async Task<bool> Delete(RoleOption roleOption)
	{
		string logInfo = $"{className} - {Names.DeleteMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			dbSet.Update(roleOption);

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
