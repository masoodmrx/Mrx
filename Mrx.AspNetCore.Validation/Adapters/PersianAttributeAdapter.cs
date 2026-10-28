namespace Mrx.AspNetCore.Validation.Adapters
{
    public class PersianAttributeAdapter : AttributeAdapterBase<PersianAttribute>
    {
        private readonly ValidationOptions options;

        public PersianAttributeAdapter(PersianAttribute attribute, ValidationOptions options)
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
            MergeAttribute(context.Attributes, "data-val-persian", GetErrorMessage(context));
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }
            //if (Attribute.ErrorMessage.IsNullOrEmpty())
            Attribute.ErrorMessage = options.Persian;

            return GetErrorMessage(
                validationContext.ModelMetadata,
                validationContext.ModelMetadata.GetDisplayName());
        }
    }
}
