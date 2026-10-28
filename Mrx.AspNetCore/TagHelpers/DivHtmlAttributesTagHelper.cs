namespace Mrx.AspNetCore.TagHelpers
{
    [HtmlTargetElement("div", Attributes = "html-attributes")]
    public class DivHtmlAttributesTagHelper : TagHelper
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
