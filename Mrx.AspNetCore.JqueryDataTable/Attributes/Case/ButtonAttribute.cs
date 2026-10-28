namespace Mrx.AspNetCore.JqueryDataTable
{
    public class ButtonAttribute : ActionCaseAttribute
    {
        public ButtonAttribute()
        {
        }

        public ButtonAttribute(string text, BootstrapColor color, string url)
        {
            Text = text;
            Class = $"btn btn-{color.GetDataValue()} " + Bootstrap.Btn.Small;
            Url = url;
        }
        public LinkTarget Target { get; set; }
        public bool Ajax { get; set; }
        public string AjaxName { get; set; }
        public string AjaxLoading { get; set; }
        private string _Attributes;
        public new string Attributes
        {
            get
            {
                string ajaxAttributes = "", targetAttributes = "";
                if (Ajax)
                {
                    ajaxAttributes = $" data-ajax='true' data-ajax-url='{Url}' data-ajax-update='#View{AjaxName}' data-ajax-loading='#Loading{AjaxLoading}' data-ajax-success='$(\"#View{AjaxName}\").initComponent();$(\"#Modal{AjaxName}\").modal(\"show\");'";
                }

                if (Target != LinkTarget.None)
                {
                    targetAttributes = $" target='{Target.GetDataValue()}'";
                }

                return _Attributes + ajaxAttributes + targetAttributes;
            }
            set => _Attributes = value;
        }
    }
}
