namespace Mrx.AspNetCore.JqueryDataTable
{
    public static class BadgeTemplateExtensions
    {
        //public static void AddEnumCases<T>(this JqueryDataTableOptions options, Func<T, BootstrapColor> func) where T : Enum
        //{
        //    options.AddEnumCases<T>(p => new BadgeAttribute(func(p)));
        //}
        public static void AddBadgeEnumCases<T>(this JqueryDataTableOptions options, Func<T, BootstrapColor> func) where T : Enum
        {
            options.AddEnumCases<T>(p => new BadgeAttribute(func(p)));
        }
    }
}
