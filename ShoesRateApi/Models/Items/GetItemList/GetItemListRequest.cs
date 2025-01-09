namespace ShoesRateApi.Models.Items.GetItemList;

public record GetItemListRequest(int PageSize = 10, int PageNumber = 1, string? Search = null);