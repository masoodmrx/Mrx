namespace Mrx.AspNetCore.FormBuilder
{
    public class InputViewModel
    {
        public string DisplayName { get; set; }
        public string Color { get; set; }
        public string OnOpen { get; set; }
        public string OnClose { get; set; }
        public InputType Type { get; set; }
        public InputFileType FileType { get; set; }
        public string Id { get; set; }
        public ModelExpression For { get; set; }
        public ModelExplorer ModelExplorer { get; set; }
        public PropertyInfo Property { get; set; }
        public bool Validation { get; set; } = true;
        public int Rows { get; set; } = 5;
        public int Cols { get; set; } = 5;
        public bool Multiple { get; set; }
        public bool Tags { get; set; }

        public string Currency { get; set; }

        public IEnumerable<SelectListItem> Items { get; set; }
        public string ItemsOptionLabel { get; set; }

        public string UploadImageDefault { get; set; }

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
