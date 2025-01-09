namespace ShoesRateApi.Entities;

public class User
{
	public Guid Id { get; set; }
	public string Email { get; set; } = default!;
	public string Username { get; set; } = default!;
	public string PasswordHash { get; set; } = default!;

	public virtual List<Item> Items { get; set; } = [];
	public virtual List<Rating> Ratings { get; set; } = [];
}