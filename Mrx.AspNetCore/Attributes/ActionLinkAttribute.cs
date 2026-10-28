namespace Mrx.AspNetCore.Attributes
{
    public class ActionLinkAttribute : ActionAttribute
    {
        public ActionLinkAttribute(string text, string url) : base(text)
        {
            Url = url;
        }
        public string Url { get; }
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
