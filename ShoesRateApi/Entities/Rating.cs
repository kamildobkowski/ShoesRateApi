namespace ShoesRateApi.Entities;

public class Rating
{
	public Guid Id { get; set; }
	public int Rate { get; set; }

	public Guid ItemId { get; set; }
	public Item Item { get; set; } = default!;
	
	public Guid UserId { get; set; }
	public User User { get; set; } = default!;
}