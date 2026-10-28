namespace Mrx.AspNetCore.JqueryDataTable
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class DateTimeTemplateAttribute : TemplateAttribute
    {
        public override string Render => $"(data) => data ? moment(data).format('{Format}') : '{DefaultText}'";
        public string Format { get; }
        public string DefaultText { get; }

        public DateTimeTemplateAttribute(string format = DateTimeFormat.ShortDatePattern, string defaultText = "")
        {
            Format = format;
            DefaultText = defaultText;
        }
    }
}
