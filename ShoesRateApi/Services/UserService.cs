using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShoesRateApi.Common;
using ShoesRateApi.Configuration;
using ShoesRateApi.Database;
using ShoesRateApi.Entities;
using ShoesRateApi.Models.Users.CreateUser;
using ShoesRateApi.Models.Users.LoginUser;
using ShoesRateApi.Services.Interfaces;

namespace ShoesRateApi.Services;

public class UserService(RatingsDbContext dbContext, IPasswordHasher<User> passwordHasher, 
	IValidator<CreateUserRequest> createUserValidator, IValidator<LoginUserRequest> loginUserValidator,
	IOptions<AuthenticationConfiguration> authConfiguration) : IUserService
{
	public async Task<Result<CreateUserResponse>> CreateUser(CreateUserRequest request)
	{
		var validationResult = await createUserValidator.ValidateAsync(request);
		if (validationResult is not null && !validationResult.IsValid)
			return Result.Failure<CreateUserResponse>(validationResult.ToProblemDetails());
		
		var user = new User
		{
			Email = request.Email,
			Username = request.Username,
			PasswordHash = passwordHasher.HashPassword(null!, request.Password)
		};
		
		dbContext.Users.Add(user);
		await dbContext.SaveChangesAsync();
		return Result.Success(new CreateUserResponse());
	}

	public async Task<Result<LoginUserResponse>> LoginUser(LoginUserRequest request)
	{
		var validationResult = await loginUserValidator.ValidateAsync(request);
		if (validationResult is not null && !validationResult.IsValid)
			return Result.Failure<LoginUserResponse>(validationResult.ToProblemDetails());

		var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Username == request.Username);
		if (user is null)
		{
			return Result.Failure<LoginUserResponse>(new ProblemDetails
			{
				Detail = "Invalid username or password"
			});
		}

		var verificationResult = passwordHasher
			.VerifyHashedPassword(user, user.PasswordHash, request.Password);
		if(verificationResult != PasswordVerificationResult.Success)
		{
			return Result.Failure<LoginUserResponse>(new ProblemDetails
			{
				Detail = "Invalid username or password"
			});
		}
		
		return Result.Success(new LoginUserResponse(Token: GenerateJwt(user)));
	}

	private string GenerateJwt(User user)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes(authConfiguration.Value.JwtKey);
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(
			[
				new Claim(ClaimTypes.Name, user.Username),
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
			]),
			Expires = DateTime.UtcNow.AddDays(2),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
			Issuer = authConfiguration.Value.JwtIssuer
		};
		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}
}