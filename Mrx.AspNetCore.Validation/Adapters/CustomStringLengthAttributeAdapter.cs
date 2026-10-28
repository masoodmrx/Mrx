namespace Mrx.AspNetCore.Validation.Adapters
{
    public class CustomStringLengthAttributeAdapter : AttributeAdapterBase<StringLengthAttribute>
    {
        private readonly string _max;
        private readonly string _min;
        private readonly ValidationOptions options;

        public CustomStringLengthAttributeAdapter(StringLengthAttribute attribute, ValidationOptions options)
            : base(attribute, null)
        {
            _max = Attribute.MaximumLength.ToString(CultureInfo.InvariantCulture);
            _min = Attribute.MinimumLength.ToString(CultureInfo.InvariantCulture);
            this.options = options;
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-length", GetErrorMessage(context));

            if (Attribute.MaximumLength != int.MaxValue)
            {
                MergeAttribute(context.Attributes, "data-val-length-max", _max);
            }

            if (Attribute.MinimumLength != 0)
            {
                MergeAttribute(context.Attributes, "data-val-length-min", _min);
            }
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }
            //if (Attribute.ErrorMessage.IsNullOrEmpty())
            Attribute.ErrorMessage = options.StringLength;

            return GetErrorMessage(
                validationContext.ModelMetadata,
                validationContext.ModelMetadata.GetDisplayName(),
                Attribute.MaximumLength,
                Attribute.MinimumLength);
        }
    }
}
