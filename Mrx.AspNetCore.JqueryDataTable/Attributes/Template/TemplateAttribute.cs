namespace Mrx.AspNetCore.JqueryDataTable
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class TemplateAttribute : MetaDataAttribute
    {
        /// <summary>
        /// Params (data, type, row, meta)
        /// </summary>
        //public virtual string Render { get; set; }
        public virtual string Render { get; }
    }
}
