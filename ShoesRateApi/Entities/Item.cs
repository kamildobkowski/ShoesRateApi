namespace ShoesRateApi.Entities;

public class Item
{
	public Guid Id { get; set; }
	public string Name { get; set; } = default!;
	public string? Description { get; set; } = default!;

	public User User { get; set; } = default!;
	public Guid UserId { get; set; }

	public virtual List<Rating> Ratings { get; set; } = [];
}