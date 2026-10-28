namespace Mrx.AspNetCore.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ActionAttribute : Attribute
    {
        public ActionAttribute(string text)
        {
            Text = text;
        }
        public string Text { get; }
        public string Class { get; set; }
        public string Attributes { get; set; }
    }
}
