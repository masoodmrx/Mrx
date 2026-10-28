using System.Globalization;

namespace Mrx.AspNetCore.JqueryDataTable
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class CurrencyTemplateAttribute : TemplateAttribute
    {
        public override string Render => $"(data) => data ? Intl.NumberFormat().format(data) + '{Currency}' : '{DefaultText}'";
        public string Currency { get; }
        public string DefaultText { get; }

        public CurrencyTemplateAttribute(string currency = null, string defaultText = "")
        {
            if (currency.IsNullOrEmpty())
            {
                currency = CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
            }
            Currency = currency;
            DefaultText = defaultText;
        }
    }
}
