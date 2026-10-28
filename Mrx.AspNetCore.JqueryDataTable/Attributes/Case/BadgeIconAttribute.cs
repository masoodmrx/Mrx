namespace Mrx.AspNetCore.JqueryDataTable
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class BadgeIconAttribute : CaseAttribute
    {
        public BadgeIconAttribute(string icon, BootstrapColor color)
        {
            Icon = icon;
            Color = color.GetDataValue();
        }
        public BadgeIconAttribute(object key, string icon, BootstrapColor color)
        {
            Key = key;
            Icon = icon;
            Color = color.GetDataValue();
        }
        public string Icon { get; }
        public string Color { get; }
    }
}
