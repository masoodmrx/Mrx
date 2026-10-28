namespace Mrx.AspNetCore.JqueryDataTable
{
    public class JqueryDataTableViewModel
    {
        public string AjaxUrl { get; set; }
        public string AjaxMethod { get; set; }
        public string MethodAjaxData { get; set; }
        public string MethodInitComplete { get; set; }
        public string MethodDrawCallback { get; set; }
        public string DefaultSort { get; set; }
        public string PagingMenu { get; set; }
        public bool Paging { get; set; }
        public bool Responsive { get; set; }
        public bool BootstrapResponsive { get; set; }
        public List<ColumnViewModel> Columns { get; set; }
        public List<string> Params { get; set; } = null;
    }
}
