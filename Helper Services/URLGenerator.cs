namespace HTMLPreviewerApp.Helper_Services
{
    using Microsoft.AspNetCore.Http;
    using System.Linq;

    public static class URLGenerator
    {
        public static string Generate(HttpRequest request, string sharableActionName, string htmlSampleId)
        {
            var scheme = request.HttpContext.Request.Scheme;
            var hostName = request.Host.Value;
            var controllerName = request.RouteValues.Select(x => x.Value).ToArray()[1];

            return $"{scheme}://{hostName}/{controllerName}/{sharableActionName}/{htmlSampleId}";
        }
    }
}
