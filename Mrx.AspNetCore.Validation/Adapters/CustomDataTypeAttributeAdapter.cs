namespace Mrx.AspNetCore.Validation.Adapters
{
    public class CustomDataTypeAttributeAdapter : AttributeAdapterBase<DataTypeAttribute>
    {
        private readonly ValidationOptions options;

        public CustomDataTypeAttributeAdapter(DataTypeAttribute attribute, string ruleName, ValidationOptions options)
            : base(attribute, null)
        {
            if (string.IsNullOrEmpty(ruleName))
            {
                throw new ArgumentException("Resources.ArgumentCannotBeNullOrEmpty", nameof(ruleName));
            }

            RuleName = ruleName;
            this.options = options;
        }

        public string RuleName { get; }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, RuleName, GetErrorMessage(context));
        }

        /// <inheritdoc/>
        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }
            if (Attribute is EmailAddressAttribute)
            {
                //if (Attribute.ErrorMessage.IsNullOrEmpty())
                Attribute.ErrorMessage = options.EmailAddress;
            }

            return GetErrorMessage(
                validationContext.ModelMetadata,
                validationContext.ModelMetadata.GetDisplayName(),
                Attribute.GetDataTypeName());
        }
    }
}
