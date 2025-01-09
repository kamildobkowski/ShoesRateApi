using System.Security.Claims;
using ShoesRateApi.Services.Interfaces;

namespace ShoesRateApi.Services;

public class UserContextService : IUserContextService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public UserContextService(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public Guid? GetUserId()
	{
		var result = Guid.TryParse(_httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
			out var userId);
		return result ? userId : null;
	}

	public string? GetUsername()
	{
		return _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
	}
}