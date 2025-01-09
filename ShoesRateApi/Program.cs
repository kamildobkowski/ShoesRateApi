using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ShoesRateApi.Configuration;
using ShoesRateApi.Database;
using ShoesRateApi.Entities;
using ShoesRateApi.Middleware;
using ShoesRateApi.Models.Items.CreateItem;
using ShoesRateApi.Models.Items.GetItemList;
using ShoesRateApi.Models.Ratings.CreateRating;
using ShoesRateApi.Models.Users.CreateUser;
using ShoesRateApi.Models.Users.LoginUser;
using ShoesRateApi.Services;
using ShoesRateApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuer = true,
		ValidateLifetime = true,
		ValidateAudience = false,
		ValidateIssuerSigningKey = true,
		ValidIssuer = builder.Configuration["Authentication:JwtIssuer"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:JwtKey"]!))
	};
});
builder.Services.AddSwaggerGen(option =>
{
	option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		In = ParameterLocation.Header,
		Description = "Please enter a valid token",
		Name = "Authorization",
		Type = SecuritySchemeType.Http,
		BearerFormat = "JWT",
		Scheme = "Bearer"
	});
	option.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				}
			},
			new List<string>()
		}
	});
});
builder.Services.AddDbContext<RatingsDbContext>(options =>
	options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IRatingService, RatingService>();
builder.Services.AddScoped<IValidator<CreateUserRequest>, CreateUserValidator>();
builder.Services.AddScoped<IValidator<LoginUserRequest>, LoginUserValidator>();
builder.Services.AddScoped<IValidator<CreateItemRequest>, CreateItemValidator>();
builder.Services.AddScoped<IValidator<GetItemListRequest>, GetItemListValidator>();
builder.Services.AddScoped<IValidator<CreateRatingRequest>, CreateRatingsValidator>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddOptions<AuthenticationConfiguration>()
	.Bind(builder.Configuration.GetRequiredSection("Authentication"));
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.MapControllers();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.Run();