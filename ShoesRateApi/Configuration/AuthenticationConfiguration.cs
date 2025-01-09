namespace ShoesRateApi.Configuration;

public class AuthenticationConfiguration
{
	public string JwtKey { get; init; } = default!;
	public string JwtIssuer { get; set; } = default!;
}