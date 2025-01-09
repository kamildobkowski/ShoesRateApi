using FluentValidation;

namespace ShoesRateApi.Models.Items.GetItemList;

public class GetItemListValidator : AbstractValidator<GetItemListRequest>
{
	public GetItemListValidator()
	{
		RuleFor(x => x.PageSize)
			.GreaterThanOrEqualTo(1);
		RuleFor(x => x.PageNumber)
			.GreaterThanOrEqualTo(1);
	}
}