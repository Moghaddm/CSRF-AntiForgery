using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN-HEADERNAME";
    options.FormFieldName = "AntiForgeryFieldName";
    options.SuppressXFrameOptionsHeader = false;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapGet("AntiForgery/Token",(IAntiforgery forgeryService,HttpContext context) => {
    var tokens = forgeryService.GetAndStoreTokens(context);
    context.Response.Cookies.Append("XSRF-TOKEN",tokens.RequestToken!,new CookieOptions { HttpOnly = false });
    return Results.Ok();

}).RequireAuthorization();

var antiForgeryService = app.Services.GetService<IAntiforgery>();

app.Use(
    async (context, next) =>
    {
        var request = context.Request.Path.Value;

        if (
            string.Equals(request, "/", StringComparison.OrdinalIgnoreCase)
            || string.Equals(request, "index.htmtl", StringComparison.OrdinalIgnoreCase)
        )
        {
            var tokenSet = antiForgeryService!.GetAndStoreTokens(context);
            context.Response.Cookies.Append(
                "XSRF-TOKEN",
                tokenSet.RequestToken!,
                new CookieOptions { HttpOnly = false }
            );
        }

        await next(context);
    }
);

app.MapRazorPages();

app.Run();
