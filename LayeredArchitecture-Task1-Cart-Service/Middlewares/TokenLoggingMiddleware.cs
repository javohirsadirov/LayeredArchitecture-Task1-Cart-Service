// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

namespace LayeredArchitectureTask1CartService.Middlewares;

/// <summary>
/// Middleware that logs the Authorization token from the request headers.
/// </summary>
public class TokenLoggingMiddleware
{
    private readonly RequestDelegate next;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenLoggingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    public TokenLoggingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    /// <summary>
    /// Invokes the middleware.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!string.IsNullOrEmpty(token))
        {
            Console.WriteLine($"TOKEN: {token}");
        }

        await this.next(context);
    }
}
