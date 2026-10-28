namespace Mrx.AspNetCore.Validation.Providers
{
    public class ValidationAttributeAdapterProvider : IValidationAttributeAdapterProvider
    {
        private readonly ValidationOptions options;

        public ValidationAttributeAdapterProvider(IOptions<ValidationOptions> options)
        {
            this.options = options.Value;
        }

        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            IAttributeAdapter adapter;

            Type type = attribute.GetType();

            if (type == typeof(CompareAttribute))
            {
                adapter = new CustomCompareAttributeAdapter((CompareAttribute)attribute, options);
            }
            else if (type == typeof(EmailAddressAttribute))
            {
                adapter = new CustomDataTypeAttributeAdapter((DataTypeAttribute)attribute, "data-val-email", options);
            }
            else if (type == typeof(MaxLengthAttribute))
            {
                adapter = new CustomMaxLengthAttributeAdapter((MaxLengthAttribute)attribute, options);
            }
            else if (type == typeof(MinLengthAttribute))
            {
                adapter = new CustomMinLengthAttributeAdapter((MinLengthAttribute)attribute, options);
            }
            else if (type == typeof(RegularExpressionAttribute))
            {
                adapter = new CustomRegularExpressionAttributeAdapter((RegularExpressionAttribute)attribute, options);
            }
            else if (type == typeof(RequiredAttribute))
            {
                adapter = new CustomRequiredAttributeAdapter((RequiredAttribute)attribute, options);
            }
            else if (type == typeof(StringLengthAttribute))
            {
                adapter = new CustomStringLengthAttributeAdapter((StringLengthAttribute)attribute, options);
            }
            else if (type == typeof(NationalCodeAttribute))
            {
                adapter = new NationalCodeAttributeAdapter((NationalCodeAttribute)attribute, options);
            }
            else if (type == typeof(PhoneNumberAttribute))
            {
                adapter = new PhoneNumberAttributeAdapter((PhoneNumberAttribute)attribute, options);
            }
            else if (type == typeof(PersianAttribute))
            {
                adapter = new PersianAttributeAdapter((PersianAttribute)attribute, options);
            }
            else if (type == typeof(FileSizeAttribute))
            {
                adapter = new FileSizeAttributeAdapter((FileSizeAttribute)attribute, options);
            }
            else if (type == typeof(FileExtensionAttribute))
            {
                adapter = new FileExtensionAttributeAdapter((FileExtensionAttribute)attribute, options);
            }
            else
            {
                adapter = null;
            }

            return adapter;
        }
    }
}
