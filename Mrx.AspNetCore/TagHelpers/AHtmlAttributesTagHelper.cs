namespace Mrx.AspNetCore.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "html-attributes")]
    public class AHtmlAttributesTagHelper : TagHelper
    {
        [HtmlAttributeName("html-attributes")]
        public Dictionary<string, string> HtmlAttributes { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            foreach ((string key, string value) in HtmlAttributes)
            {
                output.Attributes.SetAttribute(key, value);
            }
        }
    }
}
