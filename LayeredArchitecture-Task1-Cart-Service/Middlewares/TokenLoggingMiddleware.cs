namespace LayeredArchitecture_Task1_Cart_Service.Middlewares;

public class TokenLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public TokenLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!string.IsNullOrEmpty(token))
        {
            Console.WriteLine($"TOKEN: {token}");
        }

        await _next(context);
    }
}