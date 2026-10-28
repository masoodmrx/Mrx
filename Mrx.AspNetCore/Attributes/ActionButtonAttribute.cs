namespace Mrx.AspNetCore.Attributes
{
    public class ActionButtonAttribute : ActionAttribute
    {
        public ActionButtonAttribute(string text, ButtonType type = ButtonType.Submit) : base(text)
        {
            Type = type;
        }
        public ButtonType Type { get; }
        public bool ModalDismiss { get; set; } = false;
        public bool DataTableReload { get; set; } = false;
        public string DataTableReloadName { get; set; }
        private string _Attributes;
        public new string Attributes
        {
            get
            {
                string modalDismissAttributes = "";
                if (ModalDismiss)
                {
                    modalDismissAttributes = " data-dismiss='modal'";
                }

                string dataTableReloadAttributes = "";
                if (DataTableReload)
                {
                    dataTableReloadAttributes = " click-dt-reload";
                }

                if (DataTableReloadName.IsNotNullOrEmpty())
                {
                    dataTableReloadAttributes += $"='{DataTableReloadName}'";
                }

                return _Attributes + modalDismissAttributes + dataTableReloadAttributes;
            }
            set => _Attributes = value;
        }
    }
}
