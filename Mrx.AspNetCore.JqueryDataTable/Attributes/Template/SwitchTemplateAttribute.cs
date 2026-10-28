namespace Mrx.AspNetCore.JqueryDataTable
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class SwitchTemplateAttribute : TemplateAttribute
    {
        public virtual string Template => $"Case.Value";
        public override string Render
        {
            get
            {
                if (EnumType == null)
                {
                    return $"(data,type,row,meta) => Switch(data,meta,(Case)=>{Template})";
                }
                else
                {
                    IEnumerable<Dictionary<string, object>> cases = EnumType.GetFields()
                        .Where(p => p.GetCustomAttribute<CaseAttribute>() != null)
                        .Select(p => new
                        {
                            Field = p,
                            Case = p.GetCustomAttribute<CaseAttribute>()
                        })
                        .Select(p =>
                            p.Case.GetType()
                                .GetProperties()
                                .Where(d => d.Name != "TypeId")
                                .ToDictionary(d =>
                                    d.Name,
                                    d => d.Name == "Key" ? p.Field.GetValue(p) :
                                    d.Name == "Value" ? p.Field.GetCustomAttribute<DisplayAttribute>()?.Name :
                                    d.GetValue(p.Case)
                                    )
                        );
                    return $"(data,type,row,meta) => SwitchCase(data,meta,eval('{cases.ToJson()}'),(Case)=>{Template})";
                }
            }
        }
        public Type EnumType { get; }

        public SwitchTemplateAttribute(Type enumType = null)
        {
            EnumType = enumType;
        }
    }
}
