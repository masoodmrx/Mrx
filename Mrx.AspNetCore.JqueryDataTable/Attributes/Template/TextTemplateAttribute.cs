namespace Mrx.AspNetCore.JqueryDataTable
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class TextTemplateAttribute : TemplateAttribute
    {
        public override string Render => $"(data,type,row,meta) => data ? InsertValues(data, row, meta) : '{DefaultText}'";

        public string DefaultText { get; }

        public TextTemplateAttribute(string defaultText = "")
        {
            DefaultText = defaultText;
        }
    }
}
