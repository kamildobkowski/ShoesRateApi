using Microsoft.AspNetCore.Mvc;

namespace ShoesRateApi.Common;

public class Result<T> where T : class
{
	public ProblemDetails? Error { get; set; }
	public T Value { get; set; } = default!;
	public bool IsSuccess => Error is null;

	public IActionResult ToActionResult()
	{
		return IsSuccess ? new OkObjectResult(Value) : new ObjectResult(Error) { StatusCode = Error?.Status ?? 400 };
	}
}

public static class Result
{
	public static Result<T> Success<T>(T value) where T : class => new Result<T> { Value = value };
	public static Result<T> Failure<T>(ProblemDetails error) where T : class => new Result<T> { Error = error };
}