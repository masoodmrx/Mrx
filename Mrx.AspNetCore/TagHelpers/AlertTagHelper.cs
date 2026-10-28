namespace Mrx.AspNetCore.TagHelpers
{
    [HtmlTargetElement("alert", TagStructure = TagStructure.WithoutEndTag)]
    public class AlertTagHelper : TagHelper
    {
        private readonly IViewRenderService viewRender;

        public AlertTagHelper(IViewRenderService viewRender)
        {
            this.viewRender = viewRender;
        }
        [HtmlAttributeNotBound]
        public ViewDataDictionary ViewData { get; } = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
        public string Text { get; set; } = "موردی یافت نشد";
        public BootstrapColor Color { get; set; } = BootstrapColor.Danger;
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            ViewData.Model = new AlertViewModel
            {
                Text = Text,
                Color = Color
            };
            output.Content.SetHtmlContent(await viewRender.RenderToHtmlAsync("_Alert", ViewData));
        }
    }
}
