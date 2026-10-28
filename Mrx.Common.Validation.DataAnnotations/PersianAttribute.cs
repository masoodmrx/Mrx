namespace Mrx.Common.Validation.DataAnnotations
{
    public class PersianAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string input && !input.IsNullOrEmpty())
            {
                return Helper.IsValidPersian(input);
            }
            return true;
        }
    }
}
