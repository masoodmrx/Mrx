namespace Mrx.AspNetCore.JqueryDataTable
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class LinkTemplateAttribute : TemplateAttribute
    {
        public override string Render => $"(data,type,row,meta) => InsertValues(`<a href='{{{PropertyName}}}' target='_blank'>${{( data ? data : '{DefaultText}' )}}</a>`, row, meta)";

        public string PropertyName { get; }
        public string DefaultText { get; }

        public LinkTemplateAttribute(string propertyName = "Link", string defaultText = "")
        {
            PropertyName = propertyName;
            DefaultText = defaultText;
        }
    }
}
