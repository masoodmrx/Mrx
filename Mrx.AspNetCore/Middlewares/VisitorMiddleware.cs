namespace Mrx.AspNetCore.Middlewares
{
    public class VisitorMiddleware
    {
        private readonly RequestDelegate _next;

        public VisitorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, RouteService routeService, VisitorService visitorService, IWebHostEnvironment env, IOptions<SiteReportOptions> options)
        {
            //if (httpContext.Connection.sta == 404)
            //{
            //    httpContext.Response.Redirect(options.Value.PageNotFoundPath, false);
            //    return;
            //}
            try
            {
                if (!env.WebRootFileProvider.GetFileInfo(httpContext.Request.Path).Exists)
                {
                    await visitorService.VisitAsync();
                }

                await _next(httpContext);
            }
            catch (Exception ex)
            {
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = 500;
                await visitorService.RegisterExceptionAsync(ex);
                if (options.Value.InternalServerErrorPath.IsNotNullOrEmpty())
                {
                    httpContext.Response.Redirect(options.Value.InternalServerErrorPath, false);
                }
            }
        }
    }
}
