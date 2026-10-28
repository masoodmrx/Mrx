namespace Mrx.AspNetCore.Validation.Adapters
{
    public class CustomMaxLengthAttributeAdapter : AttributeAdapterBase<MaxLengthAttribute>
    {
        private readonly string _max;
        private readonly ValidationOptions options;

        public CustomMaxLengthAttributeAdapter(MaxLengthAttribute attribute, ValidationOptions options)
            : base(attribute, null)
        {
            _max = Attribute.Length.ToString(CultureInfo.InvariantCulture);
            this.options = options;
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-maxlength", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-maxlength-max", _max);
        }

        /// <inheritdoc />
        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }
            //if (Attribute.ErrorMessage.IsNullOrEmpty())
            Attribute.ErrorMessage = options.MaxLength;

            return GetErrorMessage(
                validationContext.ModelMetadata,
                validationContext.ModelMetadata.GetDisplayName(),
                Attribute.Length);
        }
    }
}
