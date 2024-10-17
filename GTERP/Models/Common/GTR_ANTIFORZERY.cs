using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

public class GTR_ANTIFORZERY
{
    private readonly RequestDelegate _next;
    private readonly IAntiforgery _antiforgery;

    public GTR_ANTIFORZERY(RequestDelegate next, IAntiforgery antiforgery)
    {
        _next = next;
        _antiforgery = antiforgery;
    }

    public async Task Invoke(HttpContext context)
    {
        if (HttpMethods.IsPost(context.Request.Method))
        {
            await _antiforgery.ValidateRequestAsync(context);
        }

        await _next(context);
    }
}
public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseAntiforgeryTokens(this IApplicationBuilder app)
    {
        return app.UseMiddleware<GTR_ANTIFORZERY>();
    }
}