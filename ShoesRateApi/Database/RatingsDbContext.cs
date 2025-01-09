using Microsoft.EntityFrameworkCore;
using ShoesRateApi.Entities;

namespace ShoesRateApi.Database;

public class RatingsDbContext : DbContext
{
	public RatingsDbContext(DbContextOptions options) : base(options) { }
	public DbSet<Item> Items { get; set; }
	public DbSet<Rating> Ratings { get; set; }
	public DbSet<User> Users { get; set; }
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder
			.Entity<Item>()
			.HasOne(x => x.User)
			.WithMany(x => x.Items);
		modelBuilder
			.Entity<Item>()
			.HasMany(x => x.Ratings)
			.WithOne(x => x.Item);
		modelBuilder
			.Entity<Item>()
			.HasKey(x => x.Id);

		modelBuilder
			.Entity<Rating>()
			.HasOne(x => x.User)
			.WithMany(x => x.Ratings);
		modelBuilder
			.Entity<Rating>()
			.HasOne(x => x.Item)
			.WithMany(x => x.Ratings);
		modelBuilder
			.Entity<Rating>()
			.HasKey(x => x.Id);

		modelBuilder
			.Entity<User>()
			.HasMany(x => x.Items)
			.WithOne(x => x.User);
		modelBuilder
			.Entity<User>()
			.HasMany(x => x.Ratings)
			.WithOne(x => x.User);
		modelBuilder
			.Entity<User>()
			.HasKey(x => x.Id);
		
		base.OnModelCreating(modelBuilder);
	}
}