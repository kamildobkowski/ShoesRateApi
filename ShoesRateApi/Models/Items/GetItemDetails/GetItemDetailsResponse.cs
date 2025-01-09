namespace ShoesRateApi.Models.Items.GetItemDetails;

public record GetItemDetailsResponse(Guid Id, string Name, string? Description, string CreatedByUserName, double AverageRating);