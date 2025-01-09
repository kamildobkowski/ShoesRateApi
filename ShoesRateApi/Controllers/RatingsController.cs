using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoesRateApi.Models.Ratings.CreateRating;
using ShoesRateApi.Models.Ratings.GetRatingList;
using ShoesRateApi.Services.Interfaces;

namespace ShoesRateApi.Controllers;

[ApiController]
[Route("api/item/{itemId}/ratings")]
public class RatingsController(IRatingService ratingService) 
	: ControllerBase
{
	[Authorize]
	[HttpPost]
	public async Task<IActionResult> CreateRating([FromRoute] Guid itemId, [FromBody] CreateRatingRequest request)
	{
		return (await ratingService.CreateRating(itemId, request)).ToActionResult();
	}
	
	[Authorize]
	[HttpDelete]
	public async Task<IActionResult> RemoveRating([FromRoute] Guid itemId)
	{
		return (await ratingService.RemoveRating(itemId)).ToActionResult();
	}
	
	[HttpGet]
	public async Task<IActionResult> GetRatingList([FromRoute] Guid itemId, [FromQuery] GetRatingListRequest request)
	{
		return (await ratingService.GetRatingList(itemId, request)).ToActionResult();
	}
}