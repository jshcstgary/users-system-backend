using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using AutoMapper;

using Common.Constants;
using Common.Models;
using Common.Models.Dto.RoleOption;

using RoleOptionES.Repository.Interfaces;
using RoleOptionES.Services.Interfaces;

namespace RoleOptionES.Services;

/// <summary>
/// Implementation of <see cref="IService"/> that manages the data on <see cref="RoleOption"/> entity.
/// </summary>
public class Service(ILogger<IService> logger, IRepository repository, IMapper mapper) : IService
{
	/// <summary>
	/// Constant string representing the base logger for the service layer.
	/// </summary>
	private const string className = $"{Names.RoleOptionModel}{Names.ServiceClass}";

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
	public async Task<RoleOptionDto> Create(RoleOptionCreateDto roleOptionCreateDto)
	{
		string logInfo = $"{className} - {Names.CreateMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			RoleOption roleOption = _mapper.Map<RoleOption>(roleOptionCreateDto);

			RoleOption newRoleOption = await _repository.Create(roleOption);

			return _mapper.Map<RoleOptionDto>(newRoleOption);
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
	public async Task<IEnumerable<RoleOptionDto>> GetAll(int limit, int offset, Expression<Func<RoleOption, bool>>? filter = null)
	{
		string logInfo = $"{className} - {Names.GetAllMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			IEnumerable<RoleOption> roleOptions = await _repository.GetAll(limit, offset, filter);

			return _mapper.Map<IEnumerable<RoleOptionDto>>(roleOptions);
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
	public async Task<RoleOptionDto?> GetOne(Expression<Func<RoleOption, bool>> filter)
	{
		string logInfo = $"{className} - {Names.GetOneMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			RoleOption? roleOption = await _repository.GetOne(filter);

			return _mapper.Map<RoleOptionDto>(roleOption);
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
	public async Task<RoleOptionDto?> Update(RoleOptionUpdateDto roleOptionUpdateDto)
	{
		string logInfo = $"{className} - {Names.UpdateMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			RoleOptionDto? roleOptionFound = await GetOne(roleOption => roleOption.Id == roleOptionUpdateDto.Id);

			if (roleOptionFound == null)
			{
				return null;
			}

			RoleOption roleOption = _mapper.Map<RoleOption>(roleOptionUpdateDto);

			roleOption.UpdatedAt = DateTime.UtcNow;

			RoleOption roleOptionUpdated = await _repository.Update(roleOption);

			return _mapper.Map<RoleOptionDto>(roleOptionUpdated);
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
			RoleOption roleOption = await _repository.GetOne(roleOption => roleOption.Id == id) ?? throw new Exception(ExceptionMessages.Null);

			if (!roleOption.Status)
			{
				throw new DbUpdateConcurrencyException(ExceptionMessages.AlreadyDeleted);
			}

			roleOption.Status = EntityStatus.Inactive;
			roleOption.UpdatedAt = DateTime.UtcNow;

			return await _repository.Delete(roleOption!);
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
