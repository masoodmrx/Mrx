namespace Mrx.AspNetCore.Validation.DataAnnotations
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        public string[] Extensions { get; set; } = new string[] { ".jpg", ".jpeg", ".png" };
        public override bool IsValid(object value)
        {
            if (value is IFormFile file)
            {
                string extension = Path.GetExtension(file.FileName);
                return Extensions.Contains(extension.ToLower());
            }
            return true;
        }
        public string GetErrorMessage(string displayName, ValidationOptions options)
        {
            if (ErrorMessage.IsNullOrEmpty())
            {
                ErrorMessage = options.FileExtension;
            }

            return string.Format(ErrorMessage, displayName, Extensions.StringJoin(" یا ").Replace(".", ""));
        }
    }
}
