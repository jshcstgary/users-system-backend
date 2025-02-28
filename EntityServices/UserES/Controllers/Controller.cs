using System.Linq.Expressions;
using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using Common.Constants;
using Common.Models;
using Common.Models.Dto.User;

using UserES.Services.Interfaces;

namespace UserES.Controllers;

/// <summary>
/// API controller for managing <see cref="User"/> entities.
/// </summary>
[Route(ControllerRoutes.User)]
[ApiController]
public class Controller(ILogger<Controller> logger, IService service) : ControllerBase()
{
	/// <summary>
	/// Constant string representing the base logger for the controller layer.
	/// </summary>
	private const string className = $"{Names.UserModel}{Names.ControllerClass}";

	/// <summary>
	/// Logger used to log messages and events.
	/// </summary>
	private readonly ILogger<Controller> _logger = logger;

	/// <summary>
	/// The service instance used for data managing.
	/// </summary>
	private readonly IService _service = service;

	/// <summary>
	/// Creates a new <see cref="User"/>.
	/// </summary>
	/// <param name="userCreateDto">The data for the creation of the <see cref="User"/>.</param>
	/// <returns>A task that represents the asynchronous operation and returns an API response with the <see cref="User"/> created.</returns>
	[HttpPost(Name = $"{className}_{Names.CreateMethod}")]
	[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse<UserDto>>> Create([FromBody] UserCreateDto userCreateDto)
	{
		string logInfo = $"{className} - {Names.CreateMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		ApiResponse<UserDto> _apiResponse = new();

		try
		{
			if (userCreateDto == null)
			{
				_logger.LogError($"{logInfo} - No data recieved, data is null.");

				_apiResponse.Title = "Data not recieved.";
				_apiResponse.Success = ApiStatus.Failed;
				_apiResponse.Status = HttpStatusCode.BadRequest;
				_apiResponse.Detail = "Object received as null.";

				return BadRequest(_apiResponse);
			}

			if (!ModelState.IsValid)
			{
				_logger.LogError($"{logInfo} - Invalid data, data has an invalid format.");

				_apiResponse.Title = "Data not valid.";
				_apiResponse.Success = ApiStatus.Failed;
				_apiResponse.Status = HttpStatusCode.BadRequest;
				_apiResponse.Detail = "The data has an invalid format.";

				return BadRequest(_apiResponse);
			}

			UserDto userDto = await _service.Create(userCreateDto);

			_logger.LogInformation($"{logInfo} - Data created successfully.");

			_apiResponse.Success = ApiStatus.Success;
			_apiResponse.Status = HttpStatusCode.Created;
			_apiResponse.Data = userDto;

			return CreatedAtRoute($"{className}_{Names.GetByIdMethod}", new
			{
				id = userDto.Id
			}, _apiResponse);
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"{logInfo} - {ex}");

			_apiResponse.Title = "Timeout.";
			_apiResponse.Success = ApiStatus.Failed;
			_apiResponse.Status = HttpStatusCode.RequestTimeout;

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (DbUpdateException ex)
		{
			string exceptionMessage = ex.InnerException!.Message;

			if (exceptionMessage.Contains("IX__UQ__USERS__"))
			{
				_logger.LogError($"{logInfo} - {ex}");

				_apiResponse.Title = "Duplicity of indexes.";
				_apiResponse.Success = ApiStatus.Failed;
				_apiResponse.Status = HttpStatusCode.BadRequest;

				if (exceptionMessage.Contains("IX__UQ__USERS__DNI"))
				{
					_apiResponse.AddError("dni", ["The DNI of the user already exists."]);
				}

				if (exceptionMessage.Contains("IX__UQ__USERS__EMAIL"))
				{
					_apiResponse.AddError("email", ["The email of the user already exists."]);
				}

				if (exceptionMessage.Contains("IX__UQ__USERS__PHONE"))
				{
					_apiResponse.AddError("phone", ["The phone of the user already exists."]);
				}

				if (exceptionMessage.Contains("IX__UQ__USERS__USERNAME"))
				{
					_apiResponse.AddError("username", ["The username of the user already exists."]);
				}

				return BadRequest(_apiResponse);
			}

			_logger.LogError($"{logInfo} - {ex}");

			_apiResponse.Title = "Internal Server Error.";
			_apiResponse.Success = ApiStatus.Failed;
			_apiResponse.Status = HttpStatusCode.InternalServerError;

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError($"{logInfo} - {ex}");

			_apiResponse.Title = "Internal Server Error.";
			_apiResponse.Success = ApiStatus.Failed;
			_apiResponse.Status = HttpStatusCode.InternalServerError;

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation($"Leaving {logInfo}");
		}
	}

	/// <summary>
	/// Gets a list of <see cref="User"/> entities.
	/// </summary>
	/// <param name="paginationParams">The paging parameters for the query.</param>
	/// <returns>A task that represents the asynchronous operation and returns an API response with the list of <see cref="User"/> entities.</returns>
	[HttpGet(Name = $"{className}_{Names.GetAllMethod}")]
	[ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDto>>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDto>>), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse<IEnumerable<UserDto>>), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAll([FromQuery] PaginationParams paginationParams)
	{
		string logInfo = $"{className} - {Names.GetAllMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		ApiResponse<IEnumerable<UserDto>> _apiResponse = new();

		try
		{
			if (!paginationParams.IsValid())
			{
				_logger.LogError($"{logInfo} - Query params are not valid.");

				_apiResponse.Title = "Query params invalid.";
				_apiResponse.Success = ApiStatus.Failed;
				_apiResponse.Status = HttpStatusCode.BadRequest;
				_apiResponse.Detail = "The query params does not have the correct format.";

				if (paginationParams.Limit <= 0)
				{
					_apiResponse.AddError("limit", ["Limit param must be a positive number."]);
				}

				if (paginationParams.Offset < 0)
				{
					_apiResponse.AddError("offset", ["Offset param must be zero or a positive number."]);
				}

				return BadRequest(_apiResponse);
			}

			Expression<Func<User, bool>>? filter = null;

			if (paginationParams.Status.HasValue)
			{
				filter = user => user.Status == paginationParams.Status;
			}

			IEnumerable<UserDto> usersDto = await _service.GetAll(paginationParams.Limit, paginationParams.Offset, filter);

			_logger.LogInformation($"{logInfo} - Data obtained.");

			_apiResponse.Success = ApiStatus.Success;
			_apiResponse.Status = HttpStatusCode.OK;
			_apiResponse.Data = usersDto;

			return Ok(_apiResponse);
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"{logInfo} - {ex}");

			_apiResponse.Title = "Timeout.";
			_apiResponse.Success = ApiStatus.Failed;
			_apiResponse.Status = HttpStatusCode.RequestTimeout;

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError($"{logInfo} - {ex}");

			_apiResponse.Title = "Internal Server Error.";
			_apiResponse.Success = ApiStatus.Failed;
			_apiResponse.Status = HttpStatusCode.InternalServerError;

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation($"Leaving {logInfo}");
		}
	}

	/// <summary>
	/// You get a <see cref="User"/> for your ID.
	/// </summary>
	/// <param name="id">The ID of the <see cref="User"/> to obtain.</param>
	/// <returns>A task that represents the asynchronous operation and returns an API response with the <see cref="User"/>.</returns>
	[HttpGet("{id}", Name = $"{className}_{Names.GetByIdMethod}")]
	[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse<UserDto>>> GetById([FromRoute] int id)
	{
		string logInfo = $"{className} - {Names.GetByIdMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		ApiResponse<UserDto> _apiResponse = new();

		try
		{
			if (id <= 0)
			{
				_logger.LogError($"{logInfo} - Id not valid, Id can not be less than or equal to 0 or null.");

				_apiResponse.Title = "Id not valid.";
				_apiResponse.Success = ApiStatus.Failed;
				_apiResponse.Status = HttpStatusCode.BadRequest;
				_apiResponse.Detail = "Id can not be less than or equal to 0 or null.";

				return BadRequest(_apiResponse);
			}

			UserDto? userDto = await _service.GetOne(user => user.Id == id);

			if (userDto == null)
			{
				_logger.LogError($"{logInfo} - {ErrorResponses.DataNotFound}");

				_apiResponse.Title = ErrorResponses.DataNotFound;
				_apiResponse.Success = ApiStatus.Failed;
				_apiResponse.Status = HttpStatusCode.NotFound;

				return NotFound(_apiResponse);
			}

			_logger.LogInformation($"{logInfo} - Data obtained.");

			_apiResponse.Success = ApiStatus.Success;
			_apiResponse.Status = HttpStatusCode.OK;
			_apiResponse.Data = userDto;

			return Ok(_apiResponse);
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"{logInfo} - {ex}");

			_apiResponse.Title = "Timeout.";
			_apiResponse.Success = ApiStatus.Failed;
			_apiResponse.Status = HttpStatusCode.RequestTimeout;

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError($"{logInfo} - {ex}");

			_apiResponse.Title = "Internal Server Error.";
			_apiResponse.Success = ApiStatus.Failed;
			_apiResponse.Status = HttpStatusCode.InternalServerError;

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation($"Leaving {logInfo}");
		}
	}

	/// <summary>
	/// Updates an existing <see cref="User"/>.
	/// </summary>
	/// <param name="userUpdateDto">The data for the update of the <see cref="User"/>.</param>
	/// <returns>A task that represents the asynchronous operation and returns an API response with the updated <see cref="User"/>.</returns>
	[HttpPut(Name = $"{className}_{Names.UpdateMethod}")]
	[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status409Conflict)]
	[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse<UserDto>>> Update([FromBody] UserUpdateDto userUpdateDto)
	{
		string logInfo = $"{className} - {Names.UpdateMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		ApiResponse<UserDto> _apiResponse = new();

		try
		{
			if (userUpdateDto == null)
			{
				_logger.LogError($"{logInfo} - No data recieved, data is null.");

				_apiResponse.Title = "Data not recieved.";
				_apiResponse.Success = ApiStatus.Failed;
				_apiResponse.Status = HttpStatusCode.BadRequest;
				_apiResponse.Detail = "Object received as null.";

				return BadRequest(_apiResponse);
			}

			if (!ModelState.IsValid)
			{
				_logger.LogError($"{logInfo} - Invalid data, data has an invalid format.");

				_apiResponse.Title = "Data not valid.";
				_apiResponse.Success = ApiStatus.Failed;
				_apiResponse.Status = HttpStatusCode.BadRequest;
				_apiResponse.Detail = "The data has an invalid format.";

				return BadRequest(_apiResponse);
			}

			UserDto? userUpdatedDto = await _service.Update(userUpdateDto);

			if (userUpdateDto == null)
			{
				_logger.LogError($"{logInfo} - {ErrorResponses.DataNotFound}");

				_apiResponse.Title = ErrorResponses.DataNotFound;
				_apiResponse.Success = ApiStatus.Failed;
				_apiResponse.Status = HttpStatusCode.NotFound;

				return NotFound(_apiResponse);
			}

			_logger.LogInformation($"{logInfo} - Data created successfully.");

			_apiResponse.Success = ApiStatus.Success;
			_apiResponse.Status = HttpStatusCode.OK;
			_apiResponse.Data = userUpdatedDto;

			return Ok(_apiResponse);
		}
		catch (DbUpdateConcurrencyException ex)
		{
			_logger.LogError($"{logInfo} - {ex}");

			_apiResponse.Title = "Inconsistent data.";
			_apiResponse.Success = ApiStatus.Failed;
			_apiResponse.Status = HttpStatusCode.Conflict;
			_apiResponse.Detail = "The record has been modified by another user.";

			return Conflict(_apiResponse);
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"{logInfo} - {ex}");

			_apiResponse.Title = "Timeout.";
			_apiResponse.Success = ApiStatus.Failed;
			_apiResponse.Status = HttpStatusCode.RequestTimeout;

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			_logger.LogError($"{logInfo} - {ex}");

			_apiResponse.Title = "Internal Server Error.";
			_apiResponse.Success = ApiStatus.Failed;
			_apiResponse.Status = HttpStatusCode.InternalServerError;

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation($"Leaving {logInfo}");
		}
	}

	/// <summary>
	/// Deletes a <see cref="User"/> by its ID.
	/// </summary>
	/// <param name="id">The ID of the <see cref="User"/> to be deleted.</param>
	/// <returns>A task that represents the asynchronous operation and returns an API response with the role option removed (if applicable).</returns>
	[HttpDelete("{id}", Name = $"{className}_{Names.DeleteMethod}")]
	[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status409Conflict)]
	[ProducesResponseType(typeof(ApiResponse<UserDto>), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse<UserDto>>> Delete([FromRoute] int id)
	{
		string logInfo = $"{className} - {Names.GetByIdMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		ApiResponse<UserDto> _apiResponse = new();

		try
		{
			if (id <= 0)
			{
				_logger.LogError($"{logInfo} - Id not valid, Id can not be less than or equal to 0 or null.");

				_apiResponse.Title = "Id not valid.";
				_apiResponse.Success = ApiStatus.Failed;
				_apiResponse.Status = HttpStatusCode.BadRequest;
				_apiResponse.Detail = "Id can not be less than or equal to 0 or null.";

				return BadRequest(_apiResponse);
			}

			bool deleteSuccess = await _service.Delete(id);

			if (!deleteSuccess)
			{
				_logger.LogError($"{logInfo} - Operation not completed.");

				_apiResponse.Title = "Operation not completed.";
				_apiResponse.Success = ApiStatus.Failed;
				_apiResponse.Status = HttpStatusCode.InternalServerError;

				return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
			}

			_logger.LogInformation($"{logInfo} - Successful elimination.");

			_apiResponse.Success = ApiStatus.Success;
			_apiResponse.Status = HttpStatusCode.OK;

			return Ok(_apiResponse);
		}
		catch (DbUpdateConcurrencyException ex)
		{
			if (ex.Message == ExceptionMessages.AlreadyDeleted)
			{
				_logger.LogError($"{logInfo} - The record is already deleted.");

				_apiResponse.Title = "Inconsistent data.";
				_apiResponse.Success = ApiStatus.Failed;
				_apiResponse.Status = HttpStatusCode.Conflict;
				_apiResponse.Detail = "The record is already deleted.";

				return Conflict(_apiResponse);
			}

			_logger.LogError($"{logInfo} - {ex}");

			_apiResponse.Title = "Inconsistent data.";
			_apiResponse.Success = ApiStatus.Failed;
			_apiResponse.Status = HttpStatusCode.Conflict;
			_apiResponse.Detail = "The record has been modified by another user.";

			return Conflict(_apiResponse);
		}
		catch (RetryLimitExceededException ex)
		{
			_logger.LogError($"{logInfo} - {ex}");

			_apiResponse.Title = "Timeout.";
			_apiResponse.Success = ApiStatus.Failed;
			_apiResponse.Status = HttpStatusCode.RequestTimeout;

			return StatusCode(StatusCodes.Status408RequestTimeout, _apiResponse);
		}
		catch (Exception ex)
		{
			if (ex.Message == ExceptionMessages.Null)
			{
				_logger.LogError($"{logInfo} - {ErrorResponses.DataNotFound}");

				_apiResponse.Title = ErrorResponses.DataNotFound;
				_apiResponse.Success = ApiStatus.Failed;
				_apiResponse.Status = HttpStatusCode.NotFound;

				return NotFound(_apiResponse);
			}

			_logger.LogError($"{logInfo} - {ex}");

			_apiResponse.Title = "Internal Server Error.";
			_apiResponse.Success = ApiStatus.Failed;
			_apiResponse.Status = HttpStatusCode.InternalServerError;

			return StatusCode(StatusCodes.Status500InternalServerError, _apiResponse);
		}
		finally
		{
			_logger.LogInformation($"Leaving {logInfo}");
		}
	}
}
