using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace ShoesRateApi.Common;

public static class ValidationResultExtensions
{
	public static ProblemDetails ToProblemDetails(this ValidationResult validationResult)
	{
		var problemDetails = new ProblemDetails
		{
			Status = StatusCodes.Status400BadRequest,
			Title = "ValidationError",
			Detail = "One or more validation errors occurred."
		};
		var dictionary = new Dictionary<string, List<string>>();
		foreach (var error in validationResult.Errors)
		{
			if (!dictionary.ContainsKey(error.PropertyName))
			{
				dictionary[error.PropertyName] = [];
			}
			dictionary[error.PropertyName].Add(error.ErrorMessage);
		}
		
		problemDetails.Extensions.Add("fields", dictionary);
		return problemDetails;
	}
}