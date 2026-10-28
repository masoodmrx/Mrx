namespace Mrx.AspNetCore.Validation.Adapters
{
    public class FileSizeAttributeAdapter : AttributeAdapterBase<FileSizeAttribute>
    {
        private readonly float length;
        private readonly ValidationOptions options;

        public FileSizeAttributeAdapter(FileSizeAttribute attribute, ValidationOptions options)
            : base(attribute, null)
        {
            length = Attribute.Length;
            this.options = options;
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-filesize", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-filesize-length", length.ToString());
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return Attribute.GetErrorMessage(validationContext.ModelMetadata.DisplayName, options);
        }
    }
}
