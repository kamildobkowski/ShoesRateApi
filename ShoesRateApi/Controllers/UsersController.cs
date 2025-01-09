using Microsoft.AspNetCore.Mvc;
using ShoesRateApi.Models.Users.CreateUser;
using ShoesRateApi.Models.Users.LoginUser;
using ShoesRateApi.Services;
using ShoesRateApi.Services.Interfaces;

namespace ShoesRateApi.Controllers;

[ApiController]
[Route("api/user")]
public class UsersController(IUserService userService) : ControllerBase
{
	[HttpPost("register")]
	public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
	{
		return (await userService.CreateUser(request)).ToActionResult();
	}

	[HttpPost("login")]
	public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest request)
	{
		return (await userService.LoginUser(request)).ToActionResult();
	}
}