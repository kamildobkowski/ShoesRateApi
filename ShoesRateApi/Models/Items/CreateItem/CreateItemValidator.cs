using FluentValidation;

namespace ShoesRateApi.Models.Items.CreateItem;

public class CreateItemValidator : AbstractValidator<CreateItemRequest>
{
	public CreateItemValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty()
			.MinimumLength(3)
			.MaximumLength(50);
		RuleFor(x => x.Description)
			.MaximumLength(300);
	}
}