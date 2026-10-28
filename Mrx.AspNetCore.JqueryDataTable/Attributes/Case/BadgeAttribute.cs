namespace Mrx.AspNetCore.JqueryDataTable
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class BadgeAttribute : CaseAttribute
    {
        public BadgeAttribute(BootstrapColor color)
        {
            Class = $"badge badge-{color.GetDataValue()}";
        }
        public BadgeAttribute(object key, string value, BootstrapColor color)
        {
            Key = key;
            Value = value;
            Class = $"badge badge-{color.GetDataValue()}";
        }
        public string Class { get; }
    }
}
