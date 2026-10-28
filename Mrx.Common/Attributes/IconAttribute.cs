namespace Mrx.Common.Attributes
{
    public class IconAttribute : Attribute
    {
        public string Icon { get; set; }
        public IconAttribute(string icon)
        {
            Icon = icon;
        }
    }
}
