using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using AutoMapper;

using Common.Constants;
using Common.Models;
using Common.Models.Dto.User;

using UserES.Repository.Interfaces;
using UserES.Services.Interfaces;

namespace UserES.Services;

/// <summary>
/// Implementation of <see cref="IService"/> that manages the data on <see cref="User"/> entity.
/// </summary>
public class Service(ILogger<IService> logger, IRepository repository, IMapper mapper) : IService
{
	/// <summary>
	/// Constant string representing the base logger for the service layer.
	/// </summary>
	private const string className = $"{Names.UserModel}{Names.ServiceClass}";

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
	public async Task<UserDto> Create(UserCreateDto userCreateDto)
	{
		string logInfo = $"{className} - {Names.CreateMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			User user = _mapper.Map<User>(userCreateDto);

			User newUser = await _repository.Create(user);

			return _mapper.Map<UserDto>(newUser);
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
	public async Task<IEnumerable<UserDto>> GetAll(int limit, int offset, Expression<Func<User, bool>>? filter = null)
	{
		string logInfo = $"{className} - {Names.GetAllMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			IEnumerable<User> users = await _repository.GetAll(limit, offset, filter);

			return _mapper.Map<IEnumerable<UserDto>>(users);
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
	public async Task<UserDto?> GetOne(Expression<Func<User, bool>> filter)
	{
		string logInfo = $"{className} - {Names.GetOneMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			User? user = await _repository.GetOne(filter);

			return _mapper.Map<UserDto>(user);
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
	public async Task<UserDto?> Update(UserUpdateDto userUpdateDto)
	{
		string logInfo = $"{className} - {Names.UpdateMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		try
		{
			User? userFound = await _repository.GetOne(user => user.Id == userUpdateDto.Id);

			if (userFound == null)
			{
				return null;
			}

			User user = _mapper.Map<User>(userUpdateDto);

			user.Password = userFound.Password;
			user.UpdatedAt = DateTime.UtcNow;

			User userUpdated = await _repository.Update(user);

			return _mapper.Map<UserDto>(userUpdated);
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
			User user = await _repository.GetOne(user => user.Id == id) ?? throw new Exception(ExceptionMessages.Null);

			if (!user.Status)
			{
				throw new DbUpdateConcurrencyException(ExceptionMessages.AlreadyDeleted);
			}

			user.Status = EntityStatus.Inactive;
			user.UpdatedAt = DateTime.UtcNow;

			return await _repository.Delete(user!);
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
