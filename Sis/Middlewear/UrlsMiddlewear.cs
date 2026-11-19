namespace Sis.Middlewear
{
    public class UrlsMiddlewear
    {
        private RequestDelegate _next;
        public readonly ILogger<UrlsMiddlewear> _logger;
        public UrlsMiddlewear(RequestDelegate next, ILogger<UrlsMiddlewear> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            var path = httpContext.Request.Path;
            if (path.Value.Contains("/api/v1/"))
            {
                
            }
            await _next(httpContext);
            //return true;

        }
    }
}
