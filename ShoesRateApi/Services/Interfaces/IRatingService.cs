using ShoesRateApi.Common;
using ShoesRateApi.Models.Ratings.CreateRating;
using ShoesRateApi.Models.Ratings.GetRatingList;
using ShoesRateApi.Models.Ratings.RemoveRating;

namespace ShoesRateApi.Services.Interfaces;

public interface IRatingService
{
	Task<Result<CreateRatingResponse>> CreateRating(Guid itemId, CreateRatingRequest request);
	Task<Result<RemoveRatingResponse>> RemoveRating(Guid itemId);
	Task<Result<GetRatingListResponse>> GetRatingList(Guid itemId, GetRatingListRequest request);
}