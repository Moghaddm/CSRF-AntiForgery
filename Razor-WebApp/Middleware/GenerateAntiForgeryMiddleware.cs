using Microsoft.AspNetCore.Antiforgery;

namespace Middleware;

public class GenerateAntiForgeryMiddleware 
{
    private readonly RequestDelegate _next;

    public GenerateAntiForgeryMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context, IAntiforgery antiForgery)
    {
       
    }
}

public static class GenerateAntiForgeryMiddlewareExtensions
{
    public static IApplicationBuilder UseAntiForgeryGenerator(this IApplicationBuilder builder) =>
        builder.UseMiddleware<GenerateAntiForgeryMiddleware>();
}
