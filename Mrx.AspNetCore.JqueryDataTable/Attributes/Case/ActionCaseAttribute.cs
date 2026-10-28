namespace Mrx.AspNetCore.JqueryDataTable
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class ActionCaseAttribute : CaseBaseAttribute
    {
        public string Attributes { get; set; }
        public string Url { get; set; }
        public string Text { get; set; }
        public string Class { get; set; }
        public string ShowPropertyName { get; set; }
        public object ShowPropertyValue { get; set; }
        public string HidePropertyName { get; set; }
        public object HidePropertyValue { get; set; }
    }
}
