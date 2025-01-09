using ShoesRateApi.Common;
using ShoesRateApi.Models.Items.CreateItem;
using ShoesRateApi.Models.Items.GetItemDetails;
using ShoesRateApi.Models.Items.GetItemList;
using ShoesRateApi.Models.Items.RemoveItem;

namespace ShoesRateApi.Services.Interfaces;

public interface IItemService
{
	Task<Result<CreateItemResponse>> CreateItem(CreateItemRequest request);
	Task<Result<RemoveItemResponse>> RemoveItem(Guid itemId);
	Task<Result<GetItemListResponse>> GetItemList(GetItemListRequest request);
	Task<Result<GetItemDetailsResponse>> GetItemDetails(Guid itemId);
}