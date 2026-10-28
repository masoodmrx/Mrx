namespace Mrx.AspNetCore.JqueryDataTable
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class NumberTemplateAttribute : TemplateAttribute
    {
        public override string Render => $"(data) => data ? Intl.NumberFormat().format(data) + '{Suffix}' : '{DefaultText}'";
        public string Suffix { get; }
        public string DefaultText { get; }

        public NumberTemplateAttribute(string suffix = null, string defaultText = "")
        {
            Suffix = suffix;
            DefaultText = defaultText;
        }
    }
}
