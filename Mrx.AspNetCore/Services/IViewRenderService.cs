namespace Mrx.AspNetCore.Services
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, ViewDataDictionary viewData);
        Task<IHtmlContent> RenderToHtmlAsync(string viewName, ViewDataDictionary viewData);
        Task<string> RenderToStringAsync(string viewName, object model = null);
        Task<IHtmlContent> RenderToHtmlAsync(string viewName, object model = null);
    }
    public class ViewRenderService : IViewRenderService
    {
        private readonly IRazorViewEngine razorViewEngine;
        private readonly ITempDataProvider tempDataProvider;
        private readonly IServiceProvider serviceProvider;
        private readonly IHtmlHelper htmlHelper;
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ViewRenderService(
            IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider,
            IHtmlHelper htmlHelper,
            IActionContextAccessor actionContextAccessor,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment webHostEnvironment
            )
        {
            this.razorViewEngine = razorViewEngine;
            this.tempDataProvider = tempDataProvider;
            this.serviceProvider = serviceProvider;
            this.htmlHelper = htmlHelper;
            this.actionContextAccessor = actionContextAccessor;
            this.httpContextAccessor = httpContextAccessor;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> RenderToStringAsync(string viewName, ViewDataDictionary viewData)
        {
            ActionContext actionContext = actionContextAccessor?.ActionContext;
            //var httpContext = httpContextAccessor.HttpContext;
            DefaultHttpContext httpContext = new DefaultHttpContext { RequestServices = serviceProvider };
            actionContext = actionContext ?? new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using (StringWriter sw = new StringWriter())
            {
                var viewResult = razorViewEngine.FindView(actionContext, viewName, false);

                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"{viewName} does not match any available view");
                }

                ViewContext viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewData,
                    new TempDataDictionary(actionContext?.HttpContext, tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                string view = sw.ToString();
                return view;
            }
        }

        public async Task<IHtmlContent> RenderToHtmlAsync(string viewName, ViewDataDictionary viewData)
        {
            return htmlHelper.Raw(await RenderToStringAsync(viewName, viewData));
        }

        public async Task<IHtmlContent> RenderToHtmlAsync(string viewName, object model = null)
        {
            return htmlHelper.Raw(await RenderToStringAsync(viewName, model));
        }

        public async Task<string> RenderToStringAsync(string viewName, object model = null)
        {
            return await RenderToStringAsync(viewName, new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            });
        }
    }
}
