namespace Mrx.AspNetCore.Validation.Adapters
{
    public class CustomCompareAttributeAdapter : AttributeAdapterBase<CompareAttribute>
    {
        private readonly string _otherProperty;
        private readonly ValidationOptions options;

        public CustomCompareAttributeAdapter(CompareAttribute attribute, ValidationOptions options)
            : base(attribute, null)
        {
            _otherProperty = "*." + attribute.OtherProperty;
            this.options = options;
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-equalto", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-equalto-other", _otherProperty);
        }

        /// <inheritdoc />
        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
            {
                throw new ArgumentNullException(nameof(validationContext));
            }

            string displayName = validationContext.ModelMetadata.GetDisplayName();
            string otherPropertyDisplayName = CompareAttributeWrapper.GetOtherPropertyDisplayName(
                validationContext,
                Attribute);

            //((CompareAttributeWrapper)Attribute).ValidationContext = validationContext;

            //if (Attribute.ErrorMessage.IsNullOrEmpty())
            Attribute.ErrorMessage = options.Compare;

            //return GetErrorMessage(validationContext.ModelMetadata, displayName, otherPropertyDisplayName);
            return string.Format(options.Compare, displayName, otherPropertyDisplayName);
        }

        // TODO: This entire class is needed because System.ComponentModel.DataAnnotations.CompareAttribute doesn't
        // populate OtherPropertyDisplayName until you call FormatErrorMessage.
        private sealed class CompareAttributeWrapper : CompareAttribute
        {
            public ModelValidationContextBase ValidationContext { get; set; }

            public CompareAttributeWrapper(CompareAttribute attribute)
                : base(attribute.OtherProperty)
            {
                // Copy settable properties from wrapped attribute. Don't reset default message accessor (set as
                // CompareAttribute constructor calls ValidationAttribute constructor) when all properties are null to
                // preserve default error message. Reset the message accessor when just ErrorMessageResourceType is
                // non-null to ensure correct InvalidOperationException.
                if (!string.IsNullOrEmpty(attribute.ErrorMessage) ||
                    !string.IsNullOrEmpty(attribute.ErrorMessageResourceName) ||
                    attribute.ErrorMessageResourceType != null)
                {
                    ErrorMessage = attribute.ErrorMessage;
                    ErrorMessageResourceName = attribute.ErrorMessageResourceName;
                    ErrorMessageResourceType = attribute.ErrorMessageResourceType;
                }
            }

            public override string FormatErrorMessage(string name)
            {
                string displayName = ValidationContext.ModelMetadata.GetDisplayName();
                return string.Format(CultureInfo.CurrentCulture,
                                     ErrorMessageString,
                                     displayName,
                                     GetOtherPropertyDisplayName(ValidationContext, this));
            }

            public static string GetOtherPropertyDisplayName(
                ModelValidationContextBase validationContext,
                CompareAttribute attribute)
            {
                // The System.ComponentModel.DataAnnotations.CompareAttribute doesn't populate the
                // OtherPropertyDisplayName until after IsValid() is called. Therefore, at the time we get
                // the error message for client validation, the display name is not populated and won't be used.
                string otherPropertyDisplayName = attribute.OtherPropertyDisplayName;
                if (otherPropertyDisplayName == null && validationContext.ModelMetadata.ContainerType != null)
                {
                    ModelMetadata otherProperty = validationContext.MetadataProvider.GetMetadataForProperty(
                        validationContext.ModelMetadata.ContainerType,
                        attribute.OtherProperty);
                    if (otherProperty != null)
                    {
                        return otherProperty.GetDisplayName();
                    }
                }

                return attribute.OtherProperty;
            }
        }
    }
}
