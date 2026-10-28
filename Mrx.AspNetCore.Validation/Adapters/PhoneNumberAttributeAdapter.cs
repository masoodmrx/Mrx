namespace Mrx.AspNetCore.Validation.Adapters
{
    public class PhoneNumberAttributeAdapter : AttributeAdapterBase<PhoneNumberAttribute>
    {
        private readonly ValidationOptions options;

        public PhoneNumberAttributeAdapter(PhoneNumberAttribute attribute, ValidationOptions options)
            : base(attribute, null)
        {
            this.options = options;
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "maxlength", "11");
            MergeAttribute(context.Attributes, "number", "");
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-phonenumber", GetErrorMessage(context));
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }
            //if (Attribute.ErrorMessage.IsNullOrEmpty())
            Attribute.ErrorMessage = options.PhoneNumber;

            return GetErrorMessage(
                validationContext.ModelMetadata,
                validationContext.ModelMetadata.GetDisplayName());
        }
    }
}
