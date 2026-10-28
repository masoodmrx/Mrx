namespace Mrx.AspNetCore.Validation.Adapters
{
    public class CustomMinLengthAttributeAdapter : AttributeAdapterBase<MinLengthAttribute>
    {
        private readonly string _min;
        private readonly ValidationOptions options;

        public CustomMinLengthAttributeAdapter(MinLengthAttribute attribute, ValidationOptions options)
            : base(attribute, null)
        {
            _min = Attribute.Length.ToString(CultureInfo.InvariantCulture);
            this.options = options;
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-minlength", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-minlength-min", _min);
        }

        /// <inheritdoc />
        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }
            //if (Attribute.ErrorMessage.IsNullOrEmpty())
            Attribute.ErrorMessage = options.MinLength;

            return GetErrorMessage(
                validationContext.ModelMetadata,
                validationContext.ModelMetadata.GetDisplayName(),
                Attribute.Length);
        }
    }
}
