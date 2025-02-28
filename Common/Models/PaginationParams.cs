namespace Common.Models;

/// <summary>
/// Provides a consistent structure for the Query Parameters used when the HTTP GET Request requires paging.
/// </summary>
public class PaginationParams
{
	/// <summary>
	/// Query parameter indicating the status of the records to be queried, it can be <c>true</c>, <c>false</c> or <c>null</c>.
	/// </summary>
	public bool? Status { get; set; }

	/// <summary>
	/// Query parameter indicating the number of results per page, it must a positive number. Default value: <c>10</c>.
	/// </summary>
	public int Limit { get; set; } = 10;

	/// <summary>
	/// Query parameter indicating the number of records to skip to start getting the results, it must be 0 or a positive number. Default value: <c>0</c>.
	/// </summary>
	public int Offset { get; set; } = 0;

	/// <summary>
	/// Valida si los valores de 'Limit' y 'Offset' son válidos.
	/// </summary>
	/// <returns>
	/// <c>true</c> if 'Limit' is greater than 0 and 'Offset' is greater than or equal to 0; otherwise, <c>false</c>.
	/// </returns>
	public bool IsValid()
	{
		return Limit > 0 && Offset >= 0;
	}
}
