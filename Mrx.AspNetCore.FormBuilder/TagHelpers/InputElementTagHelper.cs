namespace Mrx.AspNetCore.FormBuilder.TagHelpers
{
    [HtmlTargetElement("input-element", TagStructure = TagStructure.WithoutEndTag)]
    public class InputElementTagHelper : TagHelper
    {
        private readonly IViewRenderService viewRender;
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly GlobalVariableService GlobalVariable;
        private readonly IServiceProvider serviceProvider;
        private readonly IHtmlHelper htmlHelper;
        private readonly FormBuilderOptions options;

        public InputElementTagHelper(
            IViewRenderService viewRender,
            IOptions<FormBuilderOptions> options,
            IActionContextAccessor actionContextAccessor,
            GlobalVariableService globalVariableService,
            IServiceProvider serviceProvider,
            IHtmlHelper htmlHelper
            )
        {
            this.viewRender = viewRender;
            this.actionContextAccessor = actionContextAccessor;
            GlobalVariable = globalVariableService;
            this.serviceProvider = serviceProvider;
            this.htmlHelper = htmlHelper;
            this.options = options.Value;
        }
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        [HtmlAttributeNotBound]
        public ViewDataDictionary ViewData { get; } = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());

        public Dictionary<string, string> FormGroupAttributes { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> LabelAttributes { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> InputAttributes { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ValidationAttributes { get; set; } = new Dictionary<string, string>();
        public List<string> FormGroupClasses { get; set; } = new List<string>();
        public List<string> LabelClasses { get; set; } = new List<string>();
        public List<string> InputClasses { get; set; } = new List<string>();
        public List<string> ValidationClasses { get; set; } = new List<string>();


        private string Id { get; set; }
        [HtmlAttributeName("asp-for")]
        public ModelExpression For { get; set; }
        [HtmlAttributeName("asp-name")]
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool AutoInit { get; set; } = true;
        public bool Disabled { get; set; } = false;
        public bool Autocomplete { get; set; } = false;
        public bool Autofocus { get; set; } = false;
        public bool Readonly { get; set; } = false;
        public string Currency { get; set; }
        public string[] Accept { get; set; }
        public int TextEditorHeight { get; set; } = 300;
        public int Rows { get; set; } = 5;
        public int Cols { get; set; } = 5;
        public bool Multiple { get; set; }
        public bool Tags { get; set; }

        public string Class { get; set; }
        public string InputClass { get; set; }

        [HtmlAttributeName("asp-items-source")]
        public InputItemsSource ItemsSource { get; set; }
        [HtmlAttributeName("asp-items")]
        public IEnumerable<SelectListItem> Items { get; set; } = new List<SelectListItem>();
        [HtmlAttributeName("asp-items-option-label")]
        public string ItemsOptionLabel { get; set; }
        [HtmlAttributeName("asp-items-has-option-label")]
        public bool ItemsHasOptionLabel { get; set; } = true;
        [HtmlAttributeName("asp-items-service-type")]
        public Type ItemsServiceType { get; set; }
        [HtmlAttributeName("asp-items-service-method")]
        public string ItemsServiceMethod { get; set; }
        [HtmlAttributeName("asp-items-service-method-params")]
        public string[] ItemsServiceMethodParams { get; set; }
        public InputType Type { get; set; }
        public InputFileType FileType { get; set; }
        public string CustomView { get; set; }
        public bool Validation { get; set; } = true;
        public string UploadImageDefault { get; set; }
        public string Color { get; set; }
        public string OnOpen { get; set; }
        public string OnClose { get; set; }

        public bool Required { get; set; }
        public bool AjaxSelect { get; set; }
        public string AjaxSelectUrl { get; set; }
        public string AjaxSelectMethod { get; set; }
        public string[] AjaxSelectParams { get; set; }
        public string[] AjaxSelectParamsName { get; set; }

        [HtmlAttributeName("dt-reload")]
        public bool DataTableReload { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            if (For == null)
            {
                FormViewModel FormData = ViewContext.ViewData["FormData"] as FormViewModel;
                ModelExplorer ModelExplorer = FormData?.ModelExplorer ?? ViewContext.ViewData.ModelExplorer;
                string prefix = FormData?.For?.Name == null ? "" : FormData?.For?.Name + ".";
                For = new ModelExpression(prefix + Name, ModelExplorer.Properties.SingleOrDefault(p => p.Metadata.Name == Name));
            }
            Id = For.Name.Replace('.', '_').Replace('[', '_').Replace(']', '_');

            if (Type == InputType.Auto)
            {
                if (For.ModelExplorer.ModelType.IsEnum() || ItemsServiceMethod.IsNotNullOrEmpty() || ItemsServiceType != null)
                {
                    Type = InputType.Select;
                }
                else if (For.ModelExplorer.ModelType.IsNumber())
                {
                    Type = InputType.NumberText;
                }
                else if (For.ModelExplorer.ModelType.IsNullableType<bool>())
                {
                    Type = InputType.CheckBox;
                }
                else if (For.ModelExplorer.ModelType.IsNullableType<IFormFile>())
                {
                    Type = InputType.File;
                }
                else if (For.ModelExplorer.ModelType.IsNullableType<DateTime>())
                {
                    Type = InputType.DatePicker;
                }
                else if (For.ModelExplorer.ModelType.IsNullableType<TimeSpan>())
                {
                    Type = InputType.TimePicker;
                }
            }
            if (ItemsSource == InputItemsSource.Auto)
            {
                if (For.ModelExplorer.ModelType.IsEnum())
                {
                    ItemsSource = InputItemsSource.Enum;
                }
                else if (ItemsServiceType != null || ItemsServiceMethod.IsNotNullOrEmpty())
                {
                    ItemsSource = InputItemsSource.Service;
                }
                else
                {
                    ItemsSource = InputItemsSource.Static;
                }
            }
            MethodInfo methodFilter = null;
            switch (ItemsSource)
            {
                case InputItemsSource.Enum:
                    Items = htmlHelper.GetEnumSelectList(For.ModelExplorer.ModelType);
                    methodFilter = ViewContext.ViewData.ModelExplorer.ModelType.GetMethod(For.ModelExplorer.Metadata.Name + "Filter", BindingFlags.Public | BindingFlags.Static);
                    if (methodFilter != null)
                    {
                        Items = Items.Where(p => methodFilter.Invoke(null, new[] { Enum.Parse(For.ModelExplorer.ModelType.GetTypeNullableType(), p.Value) }) as bool? ?? false);
                    }
                    break;
                case InputItemsSource.Service:
                    ItemsServiceType = ItemsServiceType ?? options.ItemsServiceType;
                    object service = serviceProvider.GetService(ItemsServiceType);
                    MethodInfo method = ItemsServiceType.GetMethod(ItemsServiceMethod);
                    object[] param = method.GetParameters().Select((p, i) => ViewContext.ViewData.ModelExplorer.Model.GetPropertyValue(ItemsServiceMethodParams?[i] ?? p.Name)).ToArray();
                    //if(ItemsServiceMethodParams.IsNull())
                    //var parameters = method.GetParameters().Select(p => p.Name).ToArray();
                    //ItemsServiceMethodParams = ItemsServiceMethodParams ?? parameters;
                    //object[] param = parameters.Select(p => ItemsServiceMethodParams.Contains(p) ? ViewContext.ViewData.ModelExplorer.Model.GetPropertyValue(p) : null).ToArray();
                    if (method.ReturnType.Name == "Task`1")
                    {
                        Items = await (dynamic)method.Invoke(service, param);
                    }
                    else
                    {
                        Items = method.Invoke(service, param) as List<SelectListItem>;
                    }
                    methodFilter = ViewContext.ViewData.ModelExplorer.ModelType.GetMethod(For.ModelExplorer.Metadata.Name + "Filter", BindingFlags.Public | BindingFlags.Static);
                    if (methodFilter != null)
                    {
                        if (methodFilter.GetParameters().Count() == 1)
                            Items = Items.Where(p => methodFilter.Invoke(null, new[] { p }) as bool? ?? false);
                        else
                            Items = Items.Where(p => methodFilter.Invoke(null, new[] { p, ViewContext.ViewData.Model }) as bool? ?? false);
                    }
                    break;
            }
            if (AjaxSelectUrl.IsNotNullOrEmpty() || AjaxSelect)
            {
                if (AjaxSelectUrl.IsNullOrEmpty())
                {
                    var action = ItemsServiceType
                        .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                        .Where(fi => fi.IsLiteral && !fi.IsInitOnly &&
                         fi.FieldType == typeof(string) && fi.GetRawConstantValue().ToString() == ItemsServiceMethod)
                        .Select(p => p.Name)
                        .FirstOrDefault();
                    AjaxSelectUrl = $"/Select/{action}";
                }
                if (AjaxSelectParams == null)
                {
                    MethodInfo method = ItemsServiceType.GetMethod(ItemsServiceMethod);
                    AjaxSelectParams = method.GetParameters().Select(p => p.Name).ToArray();
                }
                InputAttributes.Add("data-items-option-label", ItemsHasOptionLabel ? ItemsOptionLabel.IsNullOrEmpty() ? For.Metadata.DisplayName : ItemsOptionLabel : null);
                InputAttributes.Add("data-ajax-select", "true");
                InputAttributes.Add("data-ajax-select-url", AjaxSelectUrl);
                InputAttributes.Add("data-ajax-select-params", $"['{AjaxSelectParams.StringJoin("','")}']");
                InputAttributes.Add("data-ajax-select-params-name", $"['{(AjaxSelectParamsName ?? AjaxSelectParams).StringJoin("','")}']");
                InputAttributes.Add("data-ajax-select-method", AjaxSelectMethod.IsNullOrEmpty() ? "Post" : AjaxSelectMethod);
            }
            if (Type == InputType.NumberText)
            {
                InputAttributes.Add("number", "");
            }

            if (Disabled)
            {
                InputAttributes.Add("disabled", "disabled");
            }

            if (Type == InputType.File)
            {
                if (Accept.IsNotNull())
                {
                    InputAttributes.Add("accept", Accept.StringJoin(","));
                }
                else if (FileType != InputFileType.File)
                {
                    InputAttributes.Add("accept", FileType.GetDataValue());
                }
            }
            if (Type == InputType.DatePicker || Type == InputType.TimePicker)
            {
                GlobalVariable["DateTimePicker"] = true;
            }

            InputAttributes.Add("autocomplete", Autocomplete ? "on" : "off");
            if (Autofocus)
            {
                InputAttributes.Add("autofocus", "");
            }

            if (Readonly)
            {
                InputAttributes.Add("readonly", "");
            }

            InputAttributes.Add("auto-init", AutoInit.ToString().ToLower());
            InputAttributes.Add("data-name", For.ModelExplorer.Metadata.DisplayName);
            if (Multiple)
            {
                InputAttributes.Add("multiple", "");
            }

            if (Tags)
            {
                InputAttributes.Add("tags", "");
            }

            switch (Type)
            {
                case InputType.File:
                    InputClasses.Add("d-none");
                    break;
                case InputType.DatePicker:
                    FormGroupAttributes.Add("id", "_" + Id);
                    FormGroupAttributes.Add("VuePersianDatetimePicker", "");
                    InputAttributes.Add("date-picker", For.Model is DateTime date && date != default ? date.ToString("yyyy/MM/dd") : "");
                    InputAttributes.Add("v-model", $"{Id}.Model");
                    //InputAttributes.Add("@click", $"{Id}.Show=true");
                    break;
                case InputType.TimePicker:
                    FormGroupAttributes.Add("id", "_" + Id);
                    FormGroupAttributes.Add("VuePersianDatetimePicker", "");
                    InputAttributes.Add("time-picker", For?.Model is TimeSpan time && time != default ? time.ToString(@"hh\:mm") : "");
                    InputAttributes.Add("v-model", $"{Id}.Model");
                    //InputAttributes.Add("@click", $"{Id}.Show=true");
                    break;
                case InputType.Currency:
                    InputAttributes.Add("currency", Currency);
                    break;
                case InputType.TextEditor:
                    GlobalVariable["TextEditor"] = true;
                    InputAttributes.Add("text-editor", "true");
                    InputAttributes.Add("text-editor-height", TextEditorHeight.ToString());
                    break;
                default:
                    break;
            }
            if (DataTableReload)
            {
                InputClasses.Add("dt-reload");
            }

            InputClasses.Add(InputClass);
            FormGroupClasses.Add(Class);

            ViewData.Model = ViewContext.ViewData.Model;
            ViewData["InputData"] = new InputViewModel
            {
                Color = options.Color.IsNull() ? Color : options.Color,
                OnOpen = OnOpen,
                OnClose = OnClose,
                DisplayName = DisplayName ?? For.Metadata.DisplayName,
                Rows = Rows,
                Cols = Cols,
                Validation = Validation,
                Type = Type,
                FileType = FileType,
                Multiple = Multiple,
                Tags = Tags,
                Id = Id,
                For = For,
                ModelExplorer = ViewContext.ViewData.ModelExplorer,
                Property = ViewContext.ViewData.ModelExplorer.ModelType.GetProperty(For.Name),
                Currency = Currency,
                Items = Items,
                ItemsOptionLabel = ItemsHasOptionLabel ? ItemsOptionLabel.IsNullOrEmpty() ? For.Metadata.DisplayName : ItemsOptionLabel : null,
                UploadImageDefault = Type == InputType.File ? (UploadImageDefault
                ?? ViewContext.ViewData.ModelExplorer.ModelType.GetPropertyValue(ViewContext.ViewData.ModelExplorer.Model, For.Name + "Default")?.ToString()
                ?? ViewContext.ViewData.ModelExplorer.ModelType.GetPropertyValue(ViewContext.ViewData.ModelExplorer.Model, For.Name + "Url")?.ToString()
                ?? options.UploadImageDefault) : null,
                FormGroupAttributes = DictionaryHelper.Merge(FormGroupAttributes, options.FormGroupAttributes),
                LabelAttributes = DictionaryHelper.Merge(LabelAttributes, options.LabelAttributes),
                InputAttributes = DictionaryHelper.Merge(InputAttributes, options.InputAttributes),
                ValidationAttributes = DictionaryHelper.Merge(ValidationAttributes, options.ValidationAttributes),
                FormGroupClasses = FormGroupClasses.Concat(options.FormGroupClasses).ToList(),
                LabelClasses = LabelClasses.Concat(options.LabelClasses).ToList(),
                InputClasses = InputClasses.Concat(options.InputClasses).ToList(),
                ValidationClasses = ValidationClasses.Concat(options.ValidationClasses).ToList()
            };
            if (Type == InputType.Custom)
            {
                output.Content.SetHtmlContent(await viewRender.RenderToHtmlAsync(CustomView, ViewData));
            }
            else
            {
                output.Content.SetHtmlContent(await viewRender.RenderToHtmlAsync("_InputElement", ViewData));
            }
        }
    }
}
