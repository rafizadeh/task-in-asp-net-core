using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Extensions
{   
    public static class HttpContextExtensions
    {
        public static string GetPath(this HttpContext httpContext)
        {
            string area = httpContext.Request.RouteValues["Area"]?.ToString();
            area = area is null ? default : $"/{area}";

            string controller = httpContext.Request.RouteValues["Controller"].ToString().ToLower();

            string action = httpContext.Request.RouteValues["Action"].ToString().ToLower();

            if (httpContext.IsAjaxRequest() && action == "filter") action = "list";

            return $"{area}/{controller}/{action}";
        }

        public static bool IsAjaxRequest(this HttpContext httpContext) =>
        httpContext?.Request?.Headers?["X-Requested-With"].ToString() == "XMLHttpRequest";

        public static T GetService<T>(this HttpContext httpContext)
        {
            return httpContext.RequestServices.GetRequiredService<T>();
        }
    }
}