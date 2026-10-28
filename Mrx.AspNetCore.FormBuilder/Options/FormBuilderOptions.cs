namespace Mrx.AspNetCore.FormBuilder
{
    public class FormBuilderOptions
    {
        public string Layout { get; set; }
        public string UploadImageDefault { get; set; }
        public Type ItemsServiceType { get; set; }
        public string Color { get; set; } = "#417DF4";

        public Dictionary<string, string> FormAttributes { get; set; }
        public List<string> FormClasses { get; set; } = new List<string>();

        public Dictionary<string, string> FormGroupAttributes { get; set; }
        public Dictionary<string, string> LabelAttributes { get; set; }
        public Dictionary<string, string> InputAttributes { get; set; }
        public Dictionary<string, string> ValidationAttributes { get; set; }

        public List<string> FormGroupClasses { get; set; } = new List<string>();
        public List<string> LabelClasses { get; set; } = new List<string>();
        public List<string> InputClasses { get; set; } = new List<string>();
        public List<string> ValidationClasses { get; set; } = new List<string>();
    }
}
