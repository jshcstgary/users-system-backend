using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using AutoMapper;

using Common.Constants;
using Common.Models;
using Common.Models.Dto.Role;

using RoleES.Repository.Interfaces;
using RoleES.Services.Interfaces;

namespace RoleES.Services;

/// <summary>
/// Implementation of <see cref="IService"/> that manages the data on <see cref="Role"/> entity.
/// </summary>
public class Service(ILogger<IService> logger, IRepository repository, IMapper mapper) : IService
{
	/// <summary>
	/// Constant string representing the base logger for the service layer.
	/// </summary>
	private const string className = $"{Names.RoleModel}{Names.ServiceClass}";

	/// <summary>
	/// Logger used to log messages and events.
	/// </summary>
	private readonly ILogger<IService> _logger = logger;

	/// <summary>
	/// The repository instance used for data access.
	/// </summary>
	private readonly IRepository _repository = repository;

	/// <summary>
	/// The mapper instance used for object mapping.
	/// </summary>
	private readonly IMapper _mapper = mapper;

	/// <inheritdoc/>
	public async Task<RoleDto> Create(RoleCreateDto roleCreateDto)
	{
		string logInfo = $"{className} - {Names.CreateMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			Role role = _mapper.Map<Role>(roleCreateDto);

			Role newRole = await _repository.Create(role);

			return _mapper.Map<RoleDto>(newRole);
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
	public async Task<IEnumerable<RoleDto>> GetAll(int limit, int offset, Expression<Func<Role, bool>>? filter = null)
	{
		string logInfo = $"{className} - {Names.GetAllMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			IEnumerable<Role> roles = await _repository.GetAll(limit, offset, filter);

			return _mapper.Map<IEnumerable<RoleDto>>(roles);
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
	public async Task<RoleDto?> GetOne(Expression<Func<Role, bool>> filter)
	{
		string logInfo = $"{className} - {Names.GetOneMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			Role? role = await _repository.GetOne(filter);

			return _mapper.Map<RoleDto>(role);
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
	public async Task<RoleDto?> Update(RoleUpdateDto roleUpdateDto)
	{
		string logInfo = $"{className} - {Names.UpdateMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			Role? roleFound = await _repository.GetOne(role => role.Id == roleUpdateDto.Id);

			if (roleFound == null)
			{
				return null;
			}

			Role role = _mapper.Map<Role>(roleUpdateDto);

			role.UpdatedAt = DateTime.UtcNow;

			Role? roleUpdated = await _repository.Update(role);

			return _mapper.Map<RoleDto>(roleUpdated);
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
	public async Task<bool> Delete(int id)
	{
		string logInfo = $"{className} - {Names.DeleteMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			Role role = await _repository.GetOne(role => role.Id == id) ?? throw new Exception(ExceptionMessages.Null);

			if (!role.Status)
			{
				throw new DbUpdateConcurrencyException(ExceptionMessages.AlreadyDeleted);
			}

			role.Status = EntityStatus.Inactive;
			role.UpdatedAt = DateTime.UtcNow;

			return await _repository.Delete(role!);
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
