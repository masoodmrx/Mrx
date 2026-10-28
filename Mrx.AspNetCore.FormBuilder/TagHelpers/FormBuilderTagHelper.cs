namespace Mrx.AspNetCore.FormBuilder.TagHelpers
{
    [HtmlTargetElement("form-builder", TagStructure = TagStructure.WithoutEndTag)]
    public class FormBuilderTagHelper : TagHelper
    {
        private readonly IViewRenderService viewRender;
        private readonly RouteService routeService;
        private readonly GlobalVariableService GlobalVariable;
        private readonly FormBuilderOptions options;

        public FormBuilderTagHelper(
            IViewRenderService viewRender,
            IOptions<FormBuilderOptions> options,
            RouteService routeService,
            GlobalVariableService globalVariableService)
        {
            this.viewRender = viewRender;
            this.routeService = routeService;
            GlobalVariable = globalVariableService;
            this.options = options.Value;
            Layout = this.options.Layout;
        }
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        //[HtmlAttributeNotBound]
        //public ViewDataDictionary ViewData { get; } = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());

        public Dictionary<string, string> FormAttributes { get; set; } = new Dictionary<string, string>();
        public List<string> FormClasses { get; set; } = new List<string>();


        public string Id { get; set; }
        public string Layout { get; set; }
        public string Class { get; set; }

        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }
        public bool AutoComplete { get; set; } = false;
        [HtmlAttributeName("asp-area")]
        public string Area { get; set; }
        [HtmlAttributeName("asp-controller")]
        public string Controller { get; set; }
        [HtmlAttributeName("asp-action")]
        public string Action { get; set; }
        public FormEnctype Enctype { get; set; }
        public FormMethod Method { get; set; } = FormMethod.Post;
        public bool FormContainer { get; set; } = true;
        public string Title { get; set; }
        public bool AutoInit { get; set; } = true;
        [HtmlAttributeName("dt-reload")]
        public bool DataTableReload { get; set; }
        public bool FormActions { get; set; } = true;
        public FormBuilderLayoutType LayoutType { get; set; }
        public ModalSize ModalSize { get; set; } = ModalSize.Default;
        public LinkTarget Target { get; set; }

        #region Ajax
        /// <summary>
        /// Must be set to true to activate unobtrusive Ajax on the target element.
        /// </summary>
        public bool Ajax { get; set; }
        /// <summary>
        /// Gets or sets the message to display in a confirmation window before a request is submitted.
        /// </summary>
        public string AjaxConfirm { get; set; }
        /// <summary>
        /// Gets or sets the HTTP request method ("Get" or "Post").
        /// </summary>
        public string AjaxMethod { get; set; }
        /// <summary>
        /// Gets or sets the mode that specifies how to insert the response into the target DOM element. Valid values are before, after and replace. Default is replace
        /// </summary>
        public string AjaxMode { get; set; }
        /// <summary>
        /// Gets or sets a value, in milliseconds, that controls the duration of the animation when showing or hiding the loading element.
        /// </summary>
        public string AjaxLoadingDuration { get; set; }
        /// <summary>
        /// Gets or sets the id attribute of an HTML element that is displayed while the Ajax function is loading.
        /// </summary>
        public string AjaxLoading { get; set; }
        /// <summary>
        /// Gets or sets the name of the JavaScript function to call immediately before the page is updated.
        /// </summary>
        public string AjaxBegin { get; set; }
        /// <summary>
        /// Gets or sets the JavaScript function to call when response data has been instantiated but before the page is updated.
        /// </summary>
        public string AjaxComplete { get; set; }
        /// <summary>
        /// Gets or sets the JavaScript function to call if the page update fails.
        /// </summary>
        public string AjaxFailure { get; set; }
        /// <summary>
        /// Gets or sets the JavaScript function to call after the page is successfully updated.
        /// </summary>
        public string AjaxSuccess { get; set; }
        /// <summary>
        /// Gets or sets the ID of the DOM element to update by using the response from the server.
        /// </summary>
        public string AjaxUpdate { get; set; }
        /// <summary>
        /// Gets or sets the URL to make the request to.
        /// </summary>
        public string AjaxUrl { get; set; }
        #endregion
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            GlobalVariable["FormBuilder"] = true;
            output.TagName = null;
            ModelExplorer ModelExplorer = For?.ModelExplorer ?? ViewContext.ViewData.ModelExplorer;
            Enctype = Enctype == FormEnctype.Auto ? (ModelExplorer.Properties.Any(p => p.ModelType == typeof(IFormFile)) ? FormEnctype.multipart_form_data : FormEnctype.application_x_www_form_urlencoded) : Enctype;
            FormAttributes.Add("id", Id ?? $"Form{ModelExplorer.ModelType.Name.Replace("ViewModel", "")}");
            FormAttributes.Add("enctype", Enctype.GetDataValue());
            FormAttributes.Add("autocomplete", AutoComplete ? "on" : "off");
            FormAttributes.Add("method", Method.ToString().ToLower());
            FormAttributes.Add("auto-init", AutoInit.ToString().ToLower());
            FormClasses.Add(Class);
            //if (Area.IsNotNullOrEmpty() || Controller.IsNotNullOrEmpty() || Action.IsNotNullOrEmpty())
            //FormAttributes.Add("action", routeService.RouteUrlFromTemplate((routeService.HasArea ? $"/{Area ?? "[a]"}" : "") + $"/{Controller ?? "[c]"}/{Action ?? "[ac]"}"));

            Layout = Layout.IsNotNullOrEmpty() && LayoutType == FormBuilderLayoutType.Custom ? Layout : LayoutType.GetDataValue();

            if (Ajax)
            {
                FormAttributes.Add("data-ajax", "true");
                if (AjaxConfirm.IsNotNullOrEmpty())
                {
                    FormAttributes.Add("data-ajax-confirm", AjaxConfirm);
                }

                if (AjaxMethod.IsNotNullOrEmpty())
                {
                    FormAttributes.Add("data-ajax-method", AjaxMethod);
                }

                if (AjaxMode.IsNotNullOrEmpty())
                {
                    FormAttributes.Add("data-ajax-mode", AjaxMode);
                }

                if (AjaxLoadingDuration.IsNotNullOrEmpty())
                {
                    FormAttributes.Add("data-ajax-loading-duration", AjaxLoadingDuration);
                }

                if (AjaxLoading.IsNotNullOrEmpty())
                {
                    FormAttributes.Add("data-ajax-loading", AjaxLoading);
                }

                if (AjaxBegin.IsNotNullOrEmpty())
                {
                    FormAttributes.Add("data-ajax-begin", AjaxBegin);
                }

                if (AjaxComplete.IsNotNullOrEmpty())
                {
                    FormAttributes.Add("data-ajax-complete", AjaxComplete);
                }

                if (AjaxFailure.IsNotNullOrEmpty())
                {
                    FormAttributes.Add("data-ajax-failure", AjaxFailure);
                }

                if (AjaxSuccess.IsNotNullOrEmpty())
                {
                    FormAttributes.Add("data-ajax-success", AjaxSuccess);
                }

                if (AjaxUpdate.IsNotNullOrEmpty())
                {
                    FormAttributes.Add("data-ajax-update", AjaxUpdate);
                }

                if (AjaxUrl.IsNotNullOrEmpty())
                {
                    FormAttributes.Add("data-ajax-url", AjaxUrl);
                }
            }
            switch (LayoutType)
            {
                case FormBuilderLayoutType.Custom:
                    break;
                case FormBuilderLayoutType.Modal:
                    FormContainer = false;
                    break;
            }

            if (Target != LinkTarget.None)
            {
                FormAttributes.Add("target", Target.GetDataValue());
            }



            //ViewData.Model = ViewContext.ViewData.Model;
            if (Title.IsNullOrEmpty())
            {
                Title = routeService.Title;
            }

            ViewContext.ViewData["FormData"] = new FormViewModel
            {
                Id = FormAttributes["id"],
                Title = Title,
                FormActions = FormActions,
                AutoInit = AutoInit,
                DataTableReload = DataTableReload,
                For = For,
                FormContainer = FormContainer,
                ModelExplorer = ModelExplorer,
                Area = Area.IsNullOrEmpty() ? routeService.AreaName : Area,
                Controller = Controller.IsNullOrEmpty() ? routeService.ControllerName : Controller,
                Action = Action.IsNullOrEmpty() ? routeService.ActionName : Action,
                Layout = Layout,
                FormAttributes = DictionaryHelper.Merge(FormAttributes, options.FormAttributes),
                FormClasses = FormClasses.Concat(options.FormClasses).ToList(),
                ModalSize = ModalSize
            };
            output.Content.SetHtmlContent(await viewRender.RenderToHtmlAsync("_FormBuilder", ViewContext.ViewData));
        }
    }
}
