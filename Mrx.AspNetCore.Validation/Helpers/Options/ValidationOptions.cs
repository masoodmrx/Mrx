namespace Mrx.AspNetCore.Validation.Helpers.Options
{
    public class ValidationOptions
    {
        public ValidationOptions()
        {
            Languages.Add("fa", new ValidationLanguage()/* { Compare = "'{0}' و '{1}' مطابقت ندارند.", MaxLength = "طول {0} حداکثر باید {1} حرف باشد.", MinLengt = "طول {0} حداقل باید {1} حرف باشد.", RegularExpression = "{0} را به درستی وارد نمایید.", Required = "لطفا {0} را وارد نمایید.", EmailAddress = "{0} را به درستی وارد نمایید.", StringLength = "طول {0} باید بین {2} تا {1} حرف باشد.", NationalCode = "{0} را بدرستی وارد نمایید.", PhoneNumber = "{0} را بدرستی وارد نمایید.", Persian = "{0} را به فارسی وارد نمایید.", FileSize = "حجم {0} انتخابی نباید بیشتر از {1} کیلوبایت باشد.", FileExtension = "نوع {0} باید {1} باشد." }*/);
            Languages.Add("en", new ValidationLanguage { Compare = "{0} is not the same with {1}.", EmailAddress = "Enter the {0} correctly.", MaxLength = "The length of {0} must be at most {1} characters.", MinLength = "The length of {0} must be at least {1} characters.", RegularExpression = "Enter the {0} correctly.", Required = "The {0} field is required.", StringLength = "The length of {0} must be between {1} and {2} characters." });
        }
        public Dictionary<string, ValidationLanguage> Languages { get; set; } = new Dictionary<string, ValidationLanguage>();

        public string Compare => Languages[CultureInfo.CurrentCulture.Name].Compare;
        public string MaxLength => Languages[CultureInfo.CurrentCulture.Name].MaxLength;
        public string MinLength => Languages[CultureInfo.CurrentCulture.Name].MinLength;
        public string RegularExpression => Languages[CultureInfo.CurrentCulture.Name].RegularExpression;
        public string Required => Languages[CultureInfo.CurrentCulture.Name].Required;
        public string EmailAddress => Languages[CultureInfo.CurrentCulture.Name].EmailAddress;
        public string StringLength => Languages[CultureInfo.CurrentCulture.Name].StringLength;
        public string NationalCode => Languages[CultureInfo.CurrentCulture.Name].NationalCode;
        public string PhoneNumber => Languages[CultureInfo.CurrentCulture.Name].PhoneNumber;
        public string Persian => Languages[CultureInfo.CurrentCulture.Name].Persian;
        public string FileExtension => Languages[CultureInfo.CurrentCulture.Name].FileExtension;
        public string FileSize => Languages[CultureInfo.CurrentCulture.Name].FileSize;
        public bool CheckModelState { get; set; } = false;
    }
    public class ValidationLanguage
    {
        public string Compare { get; set; } = "'{0}' و '{1}' مطابقت ندارند.";
        public string MaxLength { get; set; } = "طول {0} حداکثر باید {1} حرف باشد.";
        public string MinLength { get; set; } = "طول {0} حداقل باید {1} حرف باشد.";
        public string RegularExpression { get; set; } = "{0} را به درستی وارد نمایید.";
        public string Required { get; set; } = "لطفا {0} را وارد نمایید.";
        public string EmailAddress { get; set; } = "{0} را به درستی وارد نمایید.";
        public string StringLength { get; set; } = "طول {0} باید بین {2} تا {1} حرف باشد.";
        public string NationalCode { get; set; } = "{0} را بدرستی وارد نمایید.";
        public string PhoneNumber { get; set; } = "{0} را بدرستی وارد نمایید.";
        public string Persian { get; set; } = "{0} را به فارسی وارد نمایید.";
        public string FileSize { get; set; } = "حجم {0} انتخابی نباید بیشتر از {1} کیلوبایت باشد.";
        public string FileExtension { get; set; } = "نوع {0} باید {1} باشد.";
        //public string Invalid { get; set; } = "{0} نامعتبر است.";
        //public string InvalidCaptchaCode { get; set; } = "کد احراز هویت نامعتبر است.";
    }
}
