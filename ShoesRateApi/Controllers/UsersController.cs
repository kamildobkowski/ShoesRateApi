using Microsoft.AspNetCore.Mvc;
using ShoesRateApi.Models.Users.CreateUser;
using ShoesRateApi.Models.Users.LoginUser;
using ShoesRateApi.Services.Interfaces;

namespace ShoesRateApi.Controllers;

[ApiController]
[Route("api/user")]
public class UsersController(IUserService userService) : ControllerBase
{
	[HttpPost("register")]
	[ProducesResponseType(typeof(CreateUserResponse), 200)]
	public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
	{
		return (await userService.CreateUser(request)).ToActionResult();
	}

	[HttpPost("login")]
	[ProducesResponseType(typeof(LoginUserResponse), 200)]
	public async Task<IActionResult> LoginUser([FromBody] LoginUserRequest request)
	{
		return (await userService.LoginUser(request)).ToActionResult();
	}
}