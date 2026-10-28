namespace Mrx.AspNetCore.JqueryDataTable
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class BadgeTemplateAttribute : SwitchTemplateAttribute
    {
        public override string Template => $"`<span class='${{Case.Class || ''}}'>${{Case.Value}}</span>`";
        public BadgeTemplateAttribute() : base()
        {
        }
        public BadgeTemplateAttribute(Type enumType = null) : base(enumType)
        {
        }
    }
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class BadgeIconTemplateAttribute : SwitchTemplateAttribute
    {
        //public override string Template => $"`<button type='button' class='btn btn-icon btn-icon rounded-circle btn-${{Case.Color}} text-white'><i class='${{Case.Icon}}'></i></button>`";
        public override string Template => $"`<span class='badge badge-${{Case.Color}}'><i class='${{Case.Icon}}'></i></span>`";
        public BadgeIconTemplateAttribute() : base()
        {
        }

        public BadgeIconTemplateAttribute(Type enumType = null) : base(enumType)
        {
        }
    }
}
