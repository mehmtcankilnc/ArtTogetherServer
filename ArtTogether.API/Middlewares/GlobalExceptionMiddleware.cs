using ArtTogether.Application.Common;

namespace ArtTogether.API.Middlewares;

public class GlobalExceptionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message, "Unexpected error occured");

            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";

            var response = new Result { IsSuccess = false , Message = "Unexpected error occured, please try again later." };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
