namespace Mrx.AspNetCore.Validation.Adapters
{
    public class CustomRegularExpressionAttributeAdapter : AttributeAdapterBase<RegularExpressionAttribute>
    {
        private readonly ValidationOptions options;

        public CustomRegularExpressionAttributeAdapter(RegularExpressionAttribute attribute, ValidationOptions options)
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
            MergeAttribute(context.Attributes, "data-val-regex", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-regex-pattern", Attribute.Pattern);
        }

        /// <inheritdoc />
        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }
            //if (Attribute.ErrorMessage.IsNullOrEmpty())
            Attribute.ErrorMessage = options.RegularExpression;

            return GetErrorMessage(
                validationContext.ModelMetadata,
                validationContext.ModelMetadata.GetDisplayName(),
                Attribute.Pattern);
        }
    }
}
