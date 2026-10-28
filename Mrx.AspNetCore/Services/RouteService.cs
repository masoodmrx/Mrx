namespace Mrx.AspNetCore.Services
{
    [Service]
    public class RouteService
    {
        private readonly IActionContextAccessor actionContextAccessor;

        public RouteService(IActionContextAccessor actionContextAccessor)
        {
            this.actionContextAccessor = actionContextAccessor;
        }
        public ControllerActionDescriptor? ControllerActionDescriptor => ActionContext?.ActionDescriptor as ControllerActionDescriptor;
        public TypeInfo? ControllerTypeInfo => ControllerActionDescriptor?.ControllerTypeInfo;
        public MethodInfo? ActionMethodInfo => ControllerActionDescriptor?.MethodInfo;
        public ActionContext? ActionContext => actionContextAccessor?.ActionContext;
        public RouteValueDictionary? RouteDataValues => ActionContext?.RouteData?.Values;
        public string? AreaName => HasArea ? RouteDataValues?["area"]?.ToString() : "";
        public string? ControllerName => HasController ? RouteDataValues?["controller"]?.ToString() : "";
        public string? ActionName => HasAction ? RouteDataValues?["action"]?.ToString() : "";
        public string? ControllerDisplayName => ControllerTypeInfo?.GetCustomAttribute<DisplayAttribute>()?.Name ?? ControllerName;
        public string? ActionDisplayName => ActionMethodInfo?.GetCustomAttribute<DisplayAttribute>()?.Name ?? ActionName;
        public string? ControllerIcon => ControllerTypeInfo?.GetCustomAttribute<IconAttribute>()?.Icon;
        public string? ActionIcon => ActionMethodInfo?.GetCustomAttribute<IconAttribute>()?.Icon;
        public string? Title => HasAction && HasController ? ActionDisplayName + " " + ControllerDisplayName : "";
        public bool HasArea => RouteDataValues?.ContainsKey("area") ?? false;
        public bool HasController => RouteDataValues?.ContainsKey("controller") ?? false;
        public bool HasAction => RouteDataValues?.ContainsKey("action") ?? false;
        public string RouteUrlFromTemplate(string template)
        {
            if (template.IsNullOrEmpty())
            {
                return template;
            }

            template = template
                .Replace("[area]", AreaName)
                .Replace("[a]", AreaName)
                .Replace("[controller]", ControllerName)
                .Replace("[c]", ControllerName)
                .Replace("[action]", ActionName)
                .Replace("[ac]", ActionName);
            return template;
        }
    }
}