using FluentValidation;

namespace ShoesRateApi.Models.Users.LoginUser;

public class LoginUserValidator : AbstractValidator<LoginUserRequest>
{
	public LoginUserValidator()
	{
		RuleFor(x => x.Username)
			.NotEmpty();
		RuleFor(x => x.Password)
			.NotEmpty();
	}
}