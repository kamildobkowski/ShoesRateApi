using Microsoft.AspNetCore.Authentication;

namespace ShoesRateApi.Middleware;

public class ErrorHandlingMiddleware
{
	private readonly RequestDelegate _next;
	
	public ErrorHandlingMiddleware(RequestDelegate next)
	{
	    _next = next;
	}
	
	public async Task InvokeAsync(HttpContext context)
	{
	    try
	    {
	        await _next(context);
	    }
	    catch (AuthenticationFailureException ex)
	    {
	        await HandleExceptionAsync(context, ex, 401, "Authentication failed");
	    }
	    catch (Exception ex)
	    {
	        await HandleExceptionAsync(context, ex, 500, "An unexpected error occurred");
	    }
	}
	
	private static Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode, string message)
	{
	    context.Response.ContentType = "application/json";
	    context.Response.StatusCode = statusCode;
	    return context.Response.WriteAsync(new
	    {
	        StatusCode = statusCode,
	        Message = message,
	        Detailed = exception.Message
	    }.ToString());
	}
}