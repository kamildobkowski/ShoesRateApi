using FluentValidation;

namespace ShoesRateApi.Models.Ratings.CreateRating;

public class CreateRatingsValidator : AbstractValidator<CreateRatingRequest>
{
	public CreateRatingsValidator()
	{
		RuleFor(x => x.Rate)
			.InclusiveBetween(1, 10);
	}
}