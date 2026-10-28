namespace Mrx.AspNetCore.Validation.Adapters
{
    public class NationalCodeAttributeAdapter : AttributeAdapterBase<NationalCodeAttribute>
    {
        private readonly ValidationOptions options;

        public NationalCodeAttributeAdapter(NationalCodeAttribute attribute, ValidationOptions options)
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

            MergeAttribute(context.Attributes, "maxlength", "10");
            MergeAttribute(context.Attributes, "number", "");
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-nationalcode", GetErrorMessage(context));
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }
            //if (Attribute.ErrorMessage.IsNullOrEmpty())
            Attribute.ErrorMessage = options.NationalCode;

            return GetErrorMessage(
                validationContext.ModelMetadata,
                validationContext.ModelMetadata.GetDisplayName());
        }
    }
}
