namespace Mrx.AspNetCore.Validation.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class FileSizeAttribute : ValidationAttribute
    {
        public float Length { get; } = 256;
        public FileSizeAttribute(float length)
        {
            Length = length;
        }
        public override bool IsValid(object value)
        {
            if (value is IFormFile file)
            {
                if (file.Length > Length * 1024)
                {
                    return false;
                }
            }
            return true;
        }
        public string GetErrorMessage(string displayName, ValidationOptions options)
        {
            if (ErrorMessage.IsNullOrEmpty())
            {
                ErrorMessage = options.FileSize;
            }

            return string.Format(ErrorMessage, displayName, Length);
        }
    }
}
