namespace ShoesRateApi.Models.Items.GetItemList;

public class GetItemListResponse
{
	public record Item(Guid Id, string Name, double AverageRating, string CreatedByUserName);

	public List<Item> Items { get; set; } = [];
	public int ItemCount { get; set; }
	public int PageNumber { get; set; }
	public int PageSize { get; set; }
	public string? Search { get; set; }
	public bool HasNext { get; set; }
	public bool HasPrevious { get; set; }
}