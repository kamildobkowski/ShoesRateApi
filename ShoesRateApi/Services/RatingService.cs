using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoesRateApi.Common;
using ShoesRateApi.Database;
using ShoesRateApi.Entities;
using ShoesRateApi.Models.Ratings.CreateRating;
using ShoesRateApi.Models.Ratings.GetRatingList;
using ShoesRateApi.Models.Ratings.RemoveRating;
using ShoesRateApi.Services.Interfaces;

namespace ShoesRateApi.Services;

public class RatingService(RatingsDbContext dbContext,
	IValidator<CreateRatingRequest> createRatingsValidator,
	IUserContextService userContextService) : IRatingService
{
	public async Task<Result<CreateRatingResponse>> CreateRating(Guid itemId, CreateRatingRequest request)
	{
		var validationResult = await createRatingsValidator.ValidateAsync(request);
		if (validationResult is not null && !validationResult.IsValid)
		{
			return Result.Failure<CreateRatingResponse>(validationResult.ToProblemDetails());
		}

		var userId = userContextService.GetUserId();
		if (userId is null)
		{
			throw new AuthenticationFailureException("User is not authenticated");
		}

		var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId.Value);
		if (user is null)
		{
			throw new AuthenticationFailureException("User does not exist");
		}

		var isUserRatingExists = await dbContext.Ratings.AnyAsync(x => x.ItemId == itemId && x.UserId == userId.Value);
		if (isUserRatingExists)
		{
			return Result.Failure<CreateRatingResponse>(new ProblemDetails
			{
				Status = 400,
				Detail = "User has already rated this item"
			});
		}

		var rating = new Rating
		{
			ItemId = itemId,
			UserId = userId.Value,
			Rate = request.Rate
		};

		dbContext.Ratings.Add(rating);
		await dbContext.SaveChangesAsync();
		return Result.Success(new CreateRatingResponse());
	}
	
	public async Task<Result<RemoveRatingResponse>> RemoveRating(Guid itemId)
	{
		var rating = await dbContext.Ratings.FirstOrDefaultAsync(x => x.ItemId == itemId && x.UserId == userContextService.GetUserId());
		if (rating is null)
		{
			return Result.Failure<RemoveRatingResponse>(new ProblemDetails
			{
				Status = 404,
				Detail = "Rating does not exist"
			});
		}
		
		dbContext.Ratings.Remove(rating);
		await dbContext.SaveChangesAsync();
		return Result.Success(new RemoveRatingResponse());
	}
	
	public async Task<Result<GetRatingListResponse>> GetRatingList(Guid itemId, GetRatingListRequest request)
	{
		const int pageSize = 10;
		var count = dbContext.Ratings.Count(x => x.ItemId == itemId);
		var ratings = await dbContext
			.Ratings
			.Where(x => x.ItemId == itemId)
			.Include(x => x.User)
			.Select(x => new GetRatingListResponse.RatingItem(x.User.Username, x.Rate))
			.ToListAsync();
		return Result.Success(new GetRatingListResponse
		{
			Ratings = ratings,
			HasNext = request.PageNumber * pageSize < count,
			HasPrevious = request.PageNumber > 1
		});
	}
}