namespace Mrx.Common.Validation.DataAnnotations
{
    public class NationalCodeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string input && !input.IsNullOrEmpty())
            {
                return Helper.IsValidNationalCode(input);
            }
            return true;
        }
    }
}
