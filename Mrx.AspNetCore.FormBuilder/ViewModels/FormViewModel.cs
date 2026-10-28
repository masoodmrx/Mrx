namespace Mrx.AspNetCore.FormBuilder
{
    public class FormViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public ModelExpression For { get; set; }
        public Dictionary<string, string> FormAttributes { get; set; }
        public List<string> FormClasses { get; set; } = new List<string>();
        public bool FormContainer { get; set; }
        public string Layout { get; set; }
        public ModelExplorer ModelExplorer { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool AutoInit { get; set; }
        public bool DataTableReload { get; set; }
        public bool FormActions { get; set; }
        public ModalSize ModalSize { get; set; }
    }
}
