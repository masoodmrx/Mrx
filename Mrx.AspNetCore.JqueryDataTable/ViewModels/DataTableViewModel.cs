namespace Mrx.AspNetCore.JqueryDataTable
{
    public class DataTableViewModel
    {
        public string Title { get; set; }
        public ModelExplorer ModelExplorer { get; set; }
        public string Layout { get; set; }
        public Type ModelType { get; set; }
        public string TableId { get; set; }
        public string TableClass { get; set; }
        public string TableHeadClass { get; set; }
        public JqueryDataTableViewModel JqueryDataTable { get; set; }
        public ModalSize ModalSize { get; set; }
    }
}
