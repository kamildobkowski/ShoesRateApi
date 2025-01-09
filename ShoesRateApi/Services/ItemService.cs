using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoesRateApi.Common;
using ShoesRateApi.Database;
using ShoesRateApi.Entities;
using ShoesRateApi.Models.Items.CreateItem;
using ShoesRateApi.Models.Items.GetItemDetails;
using ShoesRateApi.Models.Items.GetItemList;
using ShoesRateApi.Models.Items.RemoveItem;
using ShoesRateApi.Services.Interfaces;

namespace ShoesRateApi.Services;

public class ItemService(IUserContextService userContextService, RatingsDbContext dbContext, 
	IValidator<CreateItemRequest> createItemValidator,
	IValidator<GetItemListRequest> getItemListValidator) : IItemService
{
	public async Task<Result<CreateItemResponse>> CreateItem(CreateItemRequest request)
	{
		var validation = await createItemValidator.ValidateAsync(request);
		if(validation is not null && !validation.IsValid)
			return Result.Failure<CreateItemResponse>(validation.ToProblemDetails());
		var userId = userContextService.GetUserId();
		if(userId is null)
			throw new AuthenticationFailureException("User is not authenticated");
		var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId.Value);
		if (user is null)
			throw new AuthenticationFailureException("User does not exist");

		var entity = new Item
		{
			Name = request.Name,
			Description = request.Description,
			UserId = userId.Value
		};
		dbContext.Items.Add(entity);
		await dbContext.SaveChangesAsync();
		
		return Result.Success(new CreateItemResponse(Id: entity.Id));
	}

	public async Task<Result<RemoveItemResponse>> RemoveItem(Guid itemId)
	{
		var entity = await dbContext
			.Items
			.Include(x => x.User)
			.FirstOrDefaultAsync(x => x.Id == itemId);
		
		if (entity is null)
			return Result.Failure<RemoveItemResponse>(new ProblemDetails
			{
				Detail = "Item does not exist",
				Status = 404
			});
		
		var userId = userContextService.GetUserId();
		if (userId is null || entity.UserId != userId.Value)
			return Result.Failure<RemoveItemResponse>(new ProblemDetails
			{
				Status = 403
			});
		dbContext.Remove(entity);
		await dbContext.SaveChangesAsync();
		return Result.Success(new RemoveItemResponse());
	}

	public async Task<Result<GetItemListResponse>> GetItemList(GetItemListRequest request)
	{
		var validationResult = await getItemListValidator.ValidateAsync(request);
		if(validationResult is not null && !validationResult.IsValid)
			return Result.Failure<GetItemListResponse>(validationResult.ToProblemDetails());
		var count = await dbContext.Items.CountAsync();
		if (count == 0 && request.PageNumber == 1)
			return Result.Success(new GetItemListResponse
			{
				Items = [],
				PageNumber = 1,
				PageSize = request.PageSize
			});
		
		if (count < request.PageSize * (request.PageNumber - 1) + 1)
			return Result.Failure<GetItemListResponse>(new ProblemDetails
			{
				Detail = "Page not found",
				Status = 404
			});
		
		var list = await dbContext.Items
			.Where(x => request.Search == null || request.Search == "" || x.Name.Contains(request.Search))
			.Skip(request.PageSize * (request.PageNumber - 1))
			.Take(request.PageSize)
			.Include(x => x.User)
			.Include(x => x.Ratings)
			.Select(x =>
				new GetItemListResponse.Item(x.Id, x.Name, x.Ratings.Any() ? x.Ratings.Average(rating => (double?)rating.Rate ?? 0) : 0, x.User.Username))
			.ToListAsync();
		
		return Result.Success(new GetItemListResponse
		{
			Items = list,
			ItemCount = list.Count,
			PageNumber = request.PageNumber,
			Search = request.Search,
			PageSize = request.PageSize,
			HasNext = request.PageNumber * request.PageSize < count,
			HasPrevious = request.PageNumber > 1
		});
	}
	
	public async Task<Result<GetItemDetailsResponse>> GetItemDetails(Guid itemId)
	{
		var entity = await dbContext
			.Items
			.Include(x => x.User)
			.Include(x => x.Ratings)
			.FirstOrDefaultAsync(x => x.Id == itemId);

		if (entity is null)
			return Result.Failure<GetItemDetailsResponse>(new ProblemDetails
			{
				Detail = "Item does not exist",
				Status = 404
			});

		var averageRating = entity.Ratings.Any() ? entity.Ratings.Average(rating => (double?)rating.Rate) ?? 0 : 0;

		var response = new GetItemDetailsResponse(
			entity.Id,
			entity.Name,
			entity.Description,
			entity.User.Username,
			averageRating
		);

		return Result.Success(response);
	}
}