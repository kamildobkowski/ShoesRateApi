namespace ShoesRateApi.Models.Ratings.GetRatingList;

public class GetRatingListResponse
{
	public record Item(string User, int Rating);

	public List<Item> Ratings { get; set; } = [];
	public bool HasNext { get; set; }
	public bool HasPrevious { get; set; }
}