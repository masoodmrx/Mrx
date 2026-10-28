namespace Mrx.AspNetCore.TagHelpers
{
    [HtmlTargetElement("i", Attributes = IconAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class IIconTagHelper : TagHelper
    {
        private const string IconAttributeName = "icon";

        [HtmlAttributeName(IconAttributeName)]
        public string Icon { get; set; }
        public string Class { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.Attributes.Add("class", Icon + " " + Class);
            output.TagMode = TagMode.StartTagAndEndTag;
        }
    }
}
