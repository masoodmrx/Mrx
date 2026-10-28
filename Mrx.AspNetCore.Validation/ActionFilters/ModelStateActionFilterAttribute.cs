namespace Mrx.AspNetCore.Validation.ActionFilters
{
    public class ModelStateActionFilterAttribute : ActionFilterAttribute
    {
        private readonly RouteService routeService;
        private readonly ValidationOptions options;

        public ModelStateActionFilterAttribute(
            RouteService routeService,
            IOptions<ValidationOptions> options)
        {
            this.routeService = routeService;
            this.options = options.Value;
        }
        public async override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var checkModelState = routeService.ActionMethodInfo.HasAttribute<CheckModelStateAttribute>();
            var doNotCheckModelState = routeService.ActionMethodInfo.HasAttribute<DoNotCheckModelStateAttribute>();
            if (!(checkModelState && doNotCheckModelState))
            {
                if ((checkModelState || doNotCheckModelState ? checkModelState : options.CheckModelState) && !context.ModelState.IsValid)
                {
                    context.Result = new ViewResult();
                    return;
                }
            }
            await next();
        }
    }
}
