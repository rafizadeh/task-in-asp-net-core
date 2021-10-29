using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Extensions
{
    public static class HttpContextExtensions
    {
        public static T GetService<T>(this HttpContext httpContext)
        {
            return httpContext.RequestServices.GetRequiredService<T>();
        }
    }
}