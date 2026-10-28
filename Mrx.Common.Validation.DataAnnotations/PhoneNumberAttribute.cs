namespace Mrx.Common.Validation.DataAnnotations
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string input && !input.IsNullOrEmpty())
            {
                return Helper.IsValidPhoneNumber(input);
            }
            return true;
        }
    }
}
