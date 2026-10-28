namespace Mrx.AspNetCore.JqueryDataTable.TagHelpers
{
    [HtmlTargetElement("data-table", TagStructure = TagStructure.WithoutEndTag)]
    public class DataTableTagHelper : TagHelper
    {
        private readonly IViewRenderService viewRenderService;
        private readonly JqueryDataTableOptions options;
        private readonly RouteService routeService;
        private readonly GlobalVariableService globalVariableService;

        public DataTableTagHelper(IViewRenderService viewRenderService, IOptions<JqueryDataTableOptions> options, RouteService routeService, GlobalVariableService globalVariableService)
        {
            this.viewRenderService = viewRenderService;
            this.options = options.Value;
            this.routeService = routeService;
            this.globalVariableService = globalVariableService;
            Layout = this.options.Layout;
        }
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        public string Layout { get; set; }

        public Type ModelType { get; set; }
        public Type ParamsModelType { get; set; }
        public string AjaxUrl { get; set; }
        public string AjaxMethod { get; set; }
        public string TableId { get; set; }
        public string Class { get; set; }
        public string HeadClass { get; set; }
        public string DefaultSort { get; set; }
        public string PagingMenu { get; set; }
        public bool? PagingValue { get; private set; }
        public bool Paging { get => PagingValue.Value; set => PagingValue = value; }
        public DataTableResponsive Responsive { get; set; } = DataTableResponsive.Bootstrap;

        public string MethodAjaxData { get; set; }
        public string MethodDrawCallback { get; set; }
        public string MethodInitComplete { get; set; }
        public string Title { get; set; }
        public DataTableLayoutType LayoutType { get; set; }
        public ModalSize ModalSize { get; set; } = ModalSize.Default;
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            globalVariableService["JqueryDataTable"] = true;
            if (ModelType == null) throw new Exception("ModelType is null");

            DataTableAttribute attr = ModelType.GetCustomAttribute<DataTableAttribute>();
            if (Title.IsNullOrEmpty())
            {
                Title = routeService.Title;
            }

            ViewContext.ViewData["DataTable"] = new DataTableViewModel
            {
                Title = Title,
                ModelExplorer = ViewContext.ViewData.ModelExplorer,
                Layout = Layout.IsNotNullOrEmpty() && LayoutType == DataTableLayoutType.Custom ? Layout : LayoutType.GetDataValue(),
                ModelType = ModelType,
                TableId = TableId ?? $"DataTable{ModelType.Name.Replace("ViewModel", "")}",
                TableClass = Class ?? options.TableClass,
                TableHeadClass = HeadClass ?? options.TableHeadClass,
                ModalSize = ModalSize,
                JqueryDataTable = new JqueryDataTableViewModel
                {
                    Params = (ParamsModelType ?? attr?.ParamsModelType)?.GetProperties().Select(p => p.Name).ToList(),
                    AjaxUrl = routeService.RouteUrlFromTemplate(AjaxUrl ?? attr?.AjaxUrl ?? options.AjaxUrl),
                    AjaxMethod = AjaxMethod ?? attr?.AjaxMethod ?? options.AjaxMethod,
                    MethodAjaxData = MethodAjaxData ?? attr?.MethodAjaxData,
                    MethodDrawCallback = MethodDrawCallback ?? attr?.MethodDrawCallback,
                    MethodInitComplete = MethodInitComplete ?? attr?.MethodInitComplete,
                    DefaultSort = DefaultSort ?? attr?.DefaultSort,
                    PagingMenu = PagingMenu ?? attr?.PagingMenu ?? options.PagingMenu,
                    Paging = PagingValue ?? attr?.PagingValue ?? options.Paging ?? true,
                    Responsive = Responsive == DataTableResponsive.DataTable,
                    BootstrapResponsive = Responsive == DataTableResponsive.Bootstrap,
                    Columns = ModelType.GetProperties().OrderBy(p => p.GetCustomAttribute<JqueryDataTableColumnAttribute>()?.Order).Select(p => new ColumnViewModel
                    {
                        Name = p.Name,
                        Visible = p.HasAttribute<DisplayAttribute>(),
                        Searchable = p.HasAttribute<SearchableAttribute>(),
                        Sortable = p.HasAttribute<SortableAttribute>(),
                        Render = GetRender(p),
                        ClassName = (p.GetCustomAttribute<DataTableColumnAttribute>() ?? (p.HasAttribute<DisplayAttribute>() ? new DataTableColumnAttribute() : null))?.ClassName,
                        //Cases = p.GetCustomAttributes<CaseBaseAttribute>()?
                        //      .Select(q =>
                        //          q.GetType()
                        //          .GetProperties()
                        //          .Where(d => d.Name != "TypeId")
                        //          .ToDictionary(d =>
                        //              d.Name,
                        //              d => GetCaseValue(q, d)
                        //              )
                        //          )
                        //      .ToList()
                        Cases = GetCases(p)
                    }).ToList()
                }
            };
            output.Content.SetHtmlContent(await viewRenderService.RenderToHtmlAsync("_DataTable", ViewContext.ViewData));
        }
        private object GetCaseValue(CaseBaseAttribute caseBaseAttribute, PropertyInfo propertyInfo)
        {
            switch (propertyInfo.Name)
            {
                case "Key":
                case "ShowPropertyValue":
                case "HidePropertyValue":
                    return propertyInfo.GetValue(caseBaseAttribute);
                default:
                    if (propertyInfo.GetValue(caseBaseAttribute) == null)
                    {
                        return "";
                    }
                    else
                    {
                        return routeService.RouteUrlFromTemplate(propertyInfo.GetValue(caseBaseAttribute).ToString());
                    }
            }
        }
        private List<Dictionary<string, object>> GetCases(PropertyInfo propertyInfo)
        {
            List<CaseBaseAttribute> attrs = new List<CaseBaseAttribute>();
            if (propertyInfo.GetCustomAttributes<CaseBaseAttribute>() is IEnumerable<CaseBaseAttribute> attrs1)
            {
                attrs.AddRange(attrs1);
            }
            if (propertyInfo.PropertyType.IsEnum() && options.EnumCases.FirstOrDefault(p => p.Key == propertyInfo.PropertyType.GetTypeNullableType()) is KeyValuePair<Type, List<CaseBaseAttribute>> attrs2 && attrs2.Value.IsNotNullOrEmpty())
            {
                attrs.AddRange(attrs2.Value);
            }

            List<Dictionary<string, object>> cases = attrs?
                   .Select(q =>
                       q.GetType()
                       .GetProperties()
                       .Where(d => d.Name != "TypeId")
                       .ToDictionary(d =>
                           d.Name,
                           d => GetCaseValue(q, d)
                           )
                       )
                   .ToList();
            return cases;
        }
        private string GetRender(PropertyInfo propertyInfo)
        {
            var template = propertyInfo.GetCustomAttribute<TemplateAttribute>();
            if (template == null) return null;

            if (template is ImageTemplateAttribute imageTemplate)
                imageTemplate.Default = imageTemplate.Default ?? options.DefaultImage;

            return template.Render;
        }
    }
}
