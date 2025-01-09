using ShoesRateApi.Common;
using ShoesRateApi.Models.Users.CreateUser;
using ShoesRateApi.Models.Users.LoginUser;

namespace ShoesRateApi.Services.Interfaces;

public interface IUserService
{
	Task<Result<CreateUserResponse>> CreateUser(CreateUserRequest request);
	Task<Result<LoginUserResponse>> LoginUser(LoginUserRequest request);
}