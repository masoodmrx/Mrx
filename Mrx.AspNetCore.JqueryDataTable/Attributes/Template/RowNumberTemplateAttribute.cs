namespace Mrx.AspNetCore.JqueryDataTable
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class RowNumberTemplateAttribute : TemplateAttribute
    {
        public override string Render => @"(data,type,row,meta) => `<div class='text-center font-weight-bold text-dark'>${meta.row + meta.settings._iDisplayStart + 1}</div>`";
    }
}
