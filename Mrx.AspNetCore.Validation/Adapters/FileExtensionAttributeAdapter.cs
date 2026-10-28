namespace Mrx.AspNetCore.Validation.Adapters
{
    public class FileExtensionAttributeAdapter : AttributeAdapterBase<FileExtensionAttribute>
    {
        private readonly ValidationOptions options;

        public string[] extensions { get; set; }
        public FileExtensionAttributeAdapter(FileExtensionAttribute attribute, ValidationOptions options)
            : base(attribute, null)
        {
            extensions = Attribute.Extensions;
            this.options = options;
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-fileextension", GetErrorMessage(context));
            MergeAttribute(context.Attributes, "data-val-fileextension-extensions", extensions.StringJoin(",").Replace(".", ""));
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            return Attribute.GetErrorMessage(validationContext.ModelMetadata.DisplayName, options);
        }
    }
}
