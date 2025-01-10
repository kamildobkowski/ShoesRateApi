using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoesRateApi.Models.Items.CreateItem;
using ShoesRateApi.Models.Items.GetItemDetails;
using ShoesRateApi.Models.Items.GetItemList;
using ShoesRateApi.Services.Interfaces;

namespace ShoesRateApi.Controllers;

[ApiController]
[Route("api/item")]
public class ItemsController(IItemService itemService) : ControllerBase
{
	[Authorize]
	[ProducesResponseType(typeof(CreateItemResponse), 200)]
	[HttpPost]
	public async Task<IActionResult> CreateItem([FromBody] CreateItemRequest request)
	{
		return (await itemService.CreateItem(request)).ToActionResult();
	}
	
	[Authorize]
	[HttpDelete("{itemId:guid}")]
	public async Task<IActionResult> RemoveItem([FromRoute]Guid itemId)
	{
		return (await itemService.RemoveItem(itemId)).ToActionResult();
	}
	
	[HttpGet]
	[ProducesResponseType(typeof(GetItemListResponse), 200)]
	public async Task<IActionResult> GetItemList([FromQuery] GetItemListRequest request)
	{
		return (await itemService.GetItemList(request)).ToActionResult();
	}
	
	[HttpGet("{itemId:guid}")]
	[ProducesResponseType(typeof(GetItemDetailsResponse), 200)]
	public async Task<IActionResult> GetItemDetails([FromRoute] Guid itemId)
	{
		return (await itemService.GetItemDetails(itemId)).ToActionResult();
	}
}