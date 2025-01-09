using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ShoesRateApi.Database;

namespace ShoesRateApi.Models.Users.CreateUser;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
	public CreateUserValidator(RatingsDbContext dbContext)
	{
		RuleFor(x => x.Email)
			.EmailAddress()
			.MustAsync(async (request, email, cancellationToken)
				=> !await dbContext
					.Users
					.AnyAsync(x => x.Email == email, cancellationToken: cancellationToken));
		RuleFor(x => x.Password)
			.Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$");
		RuleFor(x => x.ConfirmPassword)
			.Equal(x => x.Password);
		RuleFor(x => x.Username)
			.MustAsync(async (request, username, cancellationToken)
				=> !await dbContext
					.Users
					.AnyAsync(x => x.Username == username, cancellationToken: cancellationToken));
	}
}