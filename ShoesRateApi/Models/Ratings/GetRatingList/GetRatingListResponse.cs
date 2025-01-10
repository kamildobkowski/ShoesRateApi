namespace ShoesRateApi.Models.Ratings.GetRatingList;

public class GetRatingListResponse
{
	public record RatingItem(string User, int Rating);

	public List<RatingItem> Ratings { get; set; } = [];
	public bool HasNext { get; set; }
	public bool HasPrevious { get; set; }
}