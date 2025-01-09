namespace ShoesRateApi.Models.Users.CreateUser;

public record CreateUserRequest(string Email, string Username, string Password, string ConfirmPassword);