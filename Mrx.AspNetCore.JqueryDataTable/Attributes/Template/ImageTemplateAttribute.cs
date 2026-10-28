namespace Mrx.AspNetCore.JqueryDataTable
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ImageTemplateAttribute : TemplateAttribute
    {
        public override string Render => $@"(data) => data ? `<div class='text-center'><img src='${{data}}' class='img-cover {Class}' width='{Size}' height='{Size}' {(Default.IsNullOrEmpty() ? "" : $"onerror=\"this.src='{Default}'\"")}/></div>` : ''";
        public string Size { get; set; } = "50px";
        public string Class { get; set; }
        public string Default { get; set; }
    }
}
