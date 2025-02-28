using System.Linq.Expressions;
using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

using Common.Constants;
using Common.Models;
using Common.Models.Dto.RoleOption;

using RoleOptionES.Services.Interfaces;

namespace RoleOptionES.Controllers;

/// <summary>
/// API controller for managing <see cref="RoleOption"/> entities.
/// </summary>
[Route(ControllerRoutes.RoleOption)]
[ApiController]
public class Controller(ILogger<Controller> logger, IService service) : ControllerBase()
{
	/// <summary>
	/// Constant string representing the base logger for the controller layer.
	/// </summary>
	private const string className = $"{Names.RoleOptionModel}{Names.ControllerClass}";

	/// <summary>
	/// Logger used to log messages and events.
	/// </summary>
	private readonly ILogger<Controller> _logger = logger;

	/// <summary>
	/// The service instance used for data managing.
	/// </summary>
	private readonly IService _service = service;

	/// <summary>
	/// Creates a new <see cref="RoleOption"/>.
	/// </summary>
	/// <param name="roleOptionCreateDto">The data for the creation of the <see cref="RoleOption"/>.</param>
	/// <returns>A task that represents the asynchronous operation and returns an API response with the <see cref="RoleOption"/> created.</returns>
	[HttpPost(Name = $"{className}_{Names.CreateMethod}")]
	[ProducesResponseType(typeof(ApiResponse<RoleOptionDto>), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ApiResponse<RoleOptionDto>), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse<RoleOptionDto>), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse<RoleOptionDto>), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse<RoleOptionDto>>> Create([FromBody] RoleOptionCreateDto roleOptionCreateDto)
	{
		string logInfo = $"{className} - {Names.CreateMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		ApiResponse<RoleOptionDto> _apiResponse = new();

		try
		{
			if (roleOptionCreateDto == null)
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

			RoleOptionDto roleOptionDto = await _service.Create(roleOptionCreateDto);

			_logger.LogInformation($"{logInfo} - Data created successfully.");

			_apiResponse.Success = ApiStatus.Success;
			_apiResponse.Status = HttpStatusCode.Created;
			_apiResponse.Data = roleOptionDto;

			return CreatedAtRoute($"{className}_{Names.GetByIdMethod}", new
			{
				id = roleOptionDto.Id
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

			if (exceptionMessage.Contains("IX__UQ__ROLE_OPTIONS__"))
			{
				_logger.LogError($"{logInfo} - {ex}");

				_apiResponse.Title = "Duplicity of indexes.";
				_apiResponse.Success = ApiStatus.Failed;
				_apiResponse.Status = HttpStatusCode.BadRequest;

				if (exceptionMessage.Contains("IX__UQ__ROLE_OPTIONS__LINK"))
				{
					_apiResponse.AddError("link", ["The link of the role option already exists."]);
				}

				if (exceptionMessage.Contains("IX__UQ__ROLE_OPTIONS__NAME"))
				{
					_apiResponse.AddError("name", ["The name of the role option already exists."]);
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
	/// Gets a list of <see cref="RoleOption"/> entities.
	/// </summary>
	/// <param name="paginationParams">The paging parameters for the query.</param>
	/// <returns>A task that represents the asynchronous operation and returns an API response with the list of <see cref="RoleOption"/> entities.</returns>
	[HttpGet(Name = $"{className}_{Names.GetAllMethod}")]
	[ProducesResponseType(typeof(ApiResponse<IEnumerable<RoleOptionDto>>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse<IEnumerable<RoleOptionDto>>), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse<IEnumerable<RoleOptionDto>>), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse<IEnumerable<RoleOptionDto>>>> GetAll([FromQuery] PaginationParams paginationParams)
	{
		string logInfo = $"{className} - {Names.GetAllMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		ApiResponse<IEnumerable<RoleOptionDto>> _apiResponse = new();

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

			Expression<Func<RoleOption, bool>>? filter = null;

			if (paginationParams.Status.HasValue)
			{
				filter = roleOption => roleOption.Status == paginationParams.Status;
			}

			IEnumerable<RoleOptionDto> roleOptionsDto = await _service.GetAll(paginationParams.Limit, paginationParams.Offset, filter);

			_logger.LogInformation($"{logInfo} - Data obtained.");

			_apiResponse.Success = ApiStatus.Success;
			_apiResponse.Status = HttpStatusCode.OK;
			_apiResponse.Data = roleOptionsDto;

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
	/// You get a <see cref="RoleOption"/> for your ID.
	/// </summary>
	/// <param name="id">The ID of the <see cref="RoleOption"/> to obtain.</param>
	/// <returns>A task that represents the asynchronous operation and returns an API response with the <see cref="RoleOption"/>.</returns>
	[HttpGet("{id}", Name = $"{className}_{Names.GetByIdMethod}")]
	[ProducesResponseType(typeof(ApiResponse<RoleOptionDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse<RoleOptionDto>), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse<RoleOptionDto>), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse<RoleOptionDto>), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse<RoleOptionDto>), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse<RoleOptionDto>>> GetById([FromRoute] int id)
	{
		string logInfo = $"{className} - {Names.GetByIdMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		ApiResponse<RoleOptionDto> _apiResponse = new();

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

			RoleOptionDto? roleOptionDto = await _service.GetOne(roleOption => roleOption.Id == id);

			if (roleOptionDto == null)
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
			_apiResponse.Data = roleOptionDto;

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
	/// Updates an existing <see cref="RoleOption"/>.
	/// </summary>
	/// <param name="roleOptionUpdateDto">The data for the update of the <see cref="RoleOption"/>.</param>
	/// <returns>A task that represents the asynchronous operation and returns an API response with the updated <see cref="RoleOption"/>.</returns>
	[HttpPut(Name = $"{className}_{Names.UpdateMethod}")]
	[ProducesResponseType(typeof(ApiResponse<RoleOptionDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse<RoleOptionDto>), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse<RoleOptionDto>), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse<RoleOptionDto>), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse<RoleOptionDto>), StatusCodes.Status409Conflict)]
	[ProducesResponseType(typeof(ApiResponse<RoleOptionDto>), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse<RoleOptionDto>>> Update([FromBody] RoleOptionUpdateDto roleOptionUpdateDto)
	{
		string logInfo = $"{className} - {Names.UpdateMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		ApiResponse<RoleOptionDto> _apiResponse = new();

		try
		{
			if (roleOptionUpdateDto == null)
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

			RoleOptionDto? roleOptionUpdatedDto = await _service.Update(roleOptionUpdateDto);

			if (roleOptionUpdateDto == null)
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
			_apiResponse.Data = roleOptionUpdatedDto;

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
	/// Deletes a <see cref="RoleOption"/> by its ID.
	/// </summary>
	/// <param name="id">The ID of the <see cref="RoleOption"/> to be deleted.</param>
	/// <returns>A task that represents the asynchronous operation and returns an API response with the role option removed (if applicable).</returns>
	[HttpDelete("{id}", Name = $"{className}_{Names.DeleteMethod}")]
	[ProducesResponseType(typeof(ApiResponse<RoleOptionDto>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ApiResponse<RoleOptionDto>), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ApiResponse<RoleOptionDto>), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ApiResponse<RoleOptionDto>), StatusCodes.Status408RequestTimeout)]
	[ProducesResponseType(typeof(ApiResponse<RoleOptionDto>), StatusCodes.Status409Conflict)]
	[ProducesResponseType(typeof(ApiResponse<RoleOptionDto>), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<ApiResponse<RoleOptionDto>>> Delete([FromRoute] int id)
	{
		string logInfo = $"{className} - {Names.GetByIdMethod} method";

		_logger.LogInformation($"Executing {logInfo}.");

		ApiResponse<RoleOptionDto> _apiResponse = new();

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
