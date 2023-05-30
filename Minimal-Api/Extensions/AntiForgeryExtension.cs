using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;

namespace Extensions;

public static class AntiForgeryExtension
{
    public static TBuilder ValidateAntiForgery<TBuilder>(this TBuilder builder)
        where TBuilder : IEndpointConventionBuilder
    {
        return builder.AddEndpointFilter(
            routeHandlerFilter: async (context, next) =>
            {
                try
                {
                    var antiForgeryService =
                        context.HttpContext.RequestServices.GetRequiredService<IAntiforgery>();
                    await antiForgeryService.ValidateRequestAsync(context.HttpContext);
                }
                catch (AntiforgeryValidationException)
                {
                    return Results.BadRequest("Anti Forgery Validation Failed !");
                }
                return await next(context);
            }
        );
    }
}
