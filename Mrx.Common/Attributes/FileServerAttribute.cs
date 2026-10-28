namespace Mrx.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class FileServerAttribute : Attribute
    {
        public string DirectoryName { get; set; }
        public string DefaultFileName { get; set; }
        public string PropertyName { get; set; }

        public FileServerAttribute() { }
        public FileServerAttribute(string directoryName)
        {
            DirectoryName = directoryName;
        }
    }
}
