namespace Mrx.AspNetCore.JqueryDataTable
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class DataTableAttribute : MetaDataAttribute
    {
        public string AjaxUrl { get; set; }
        public string AjaxMethod { get; set; }
        public string MethodAjaxData { get; set; }
        public string MethodInitComplete { get; set; }
        public string MethodDrawCallback { get; set; }
        /// <summary>
        /// [[1,'desc']] or [[1,'asc']]
        /// </summary>
        public string DefaultSort { get; set; }
        /// <summary>
        /// [[10, 25, 50, -1], [10, 25, 50, "همه"]] or [10, 25, 50]
        /// </summary>
        public string PagingMenu { get; set; }
        public bool? PagingValue { get; private set; }
        public bool Paging { get => PagingValue.Value; set => PagingValue = value; }
        public Type ParamsModelType { get; set; } = null;
    }
}
