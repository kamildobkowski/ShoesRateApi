namespace ShoesRateApi.Services.Interfaces;

public interface IUserContextService
{
	Guid? GetUserId();
	string? GetUsername();
}