namespace Mrx.AspNetCore.JqueryDataTable
{
    public class JqueryDataTableOptions
    {
        public string Layout { get; set; }
        public string AjaxUrl { get; set; }
        public string AjaxMethod { get; set; }
        public string TableClass { get; set; }
        public string TableHeadClass { get; set; }
        public bool? Paging { get; set; }
        public string PagingMenu { get; set; }
        public string DefaultImage { get; set; }
        public Dictionary<Type, List<CaseBaseAttribute>> EnumCases { get; } = new Dictionary<Type, List<CaseBaseAttribute>>();
        public Func<object, CaseBaseAttribute> FuncEnumCaseDefault { get; set; } = p => new BadgeAttribute(BootstrapColor.Info);
        public void AddEnumCases(Type enumType, Func<object, CaseBaseAttribute> func = null)
        {
            func = func ?? FuncEnumCaseDefault;
            var items = new List<CaseBaseAttribute>();
            foreach (var value in enumType.GetEnumValues())
                items.Add(func(value));
            EnumCases.Add(enumType, items);
        }
        public void AddEnumCases<T>(Func<T, CaseBaseAttribute> func) where T : Enum
        {
            Type enumType = typeof(T);
            var items = new List<CaseBaseAttribute>();
            foreach (var value in enumType.GetEnumValues())
            {
                var item = func.Method.Invoke(func.Target, new[] { value }) as CaseAttribute;
                item.Key = item.Key ?? value;
                item.Value = item.Value ?? enumType.GetMember(value.ToString()).FirstOrDefault(m => m.DeclaringType == enumType).GetDisplayName();
                items.Add(item);
            }
            EnumCases.Add(enumType, items);
        }
        public void AddDefaultEnumCases<T>() where T : Enum
        {
            AddEnumCases(typeof(T));
        }
        //public void AddEnumCases<T>(Func<object, CaseBaseAttribute> func = null) where T : Enum
        //{
        //    AddEnumCases(typeof(T), func);
        //}
    }
}
