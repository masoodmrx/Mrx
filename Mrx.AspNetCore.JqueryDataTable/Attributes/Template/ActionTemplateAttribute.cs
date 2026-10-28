namespace Mrx.AspNetCore.JqueryDataTable
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ActionTemplateAttribute : TemplateAttribute
    {
        public virtual string Template => "`<a ${Case.Attributes ? InsertValues(Case.Attributes, row, meta) : ''} href='${InsertValues(Case.Url, row, meta)}' class='${Case.Class || ''}'>${Case.Text}</a>`";
        public override string Render
        {
            get
            {
                string conditions = ".where(p => p.ShowPropertyName ? (row[p.ShowPropertyName] == p.ShowPropertyValue) : true).where(p => p.HidePropertyName ? !(row[p.HidePropertyName] == p.HidePropertyValue) : true)";
                string template = $"`${{Enumerable.from(Actions){conditions}.select(Case => {Template} ).toJoinedString()}}`";
                return $"(data,type,row,meta) => Action(meta,(Actions)=>{template})";
            }
        }
    }
}
