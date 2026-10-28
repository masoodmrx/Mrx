namespace Mrx.AspNetCore.JqueryDataTable
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public class CaseAttribute : CaseBaseAttribute
    {
        public object Key { get; set; }
        public string Value { get; set; }
    }
}
