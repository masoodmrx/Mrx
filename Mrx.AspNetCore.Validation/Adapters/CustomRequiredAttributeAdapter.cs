namespace Mrx.AspNetCore.Validation.Adapters
{
    public class CustomRequiredAttributeAdapter : AttributeAdapterBase<RequiredAttribute>
    {
        private readonly ValidationOptions options;

        public CustomRequiredAttributeAdapter(RequiredAttribute attribute, ValidationOptions options)
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

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-required", GetErrorMessage(context));
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }
            //if (Attribute.ErrorMessage.IsNullOrEmpty())
            Attribute.ErrorMessage = options.Required;

            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName());
        }
    }
}
