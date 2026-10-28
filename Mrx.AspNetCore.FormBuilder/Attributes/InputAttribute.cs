namespace Mrx.AspNetCore.FormBuilder
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class InputAttribute : Attribute
    {
        public string Color { get; set; }
        public string OnOpen { get; set; }
        public string OnClose { get; set; }
        public bool Ignore { get; set; } = false;
        public string PropertyIgnore { get; set; }
        public InputType Type { get; set; }
        public InputFileType FileType { get; set; }
        public string CustomView { get; set; }
        public bool Validation { get; set; } = true;
        public int TextEditorHeight { get; set; } = 300;
        public int Rows { get; set; } = 5;
        public int Cols { get; set; } = 5;
        public bool Multiple { get; set; }
        public bool Tags { get; set; }

        public bool Disabled { get; set; } = false;
        public bool? AutoInit { get; set; } = null;
        public bool Autocomplete { get; set; } = false;
        public bool Autofocus { get; set; } = false;
        public bool Readonly { get; set; } = false;
        public string Currency { get; set; }
        public string[] Accept { get; set; }

        public string Class { get; set; } = Bootstrap.Grid.ColMd4;
        public string InputClass { get; set; }

        public InputItemsSource ItemsSource { get; set; }
        public IEnumerable<SelectListItem> Items { get; set; } = new List<SelectListItem>();
        public string ItemsOptionLabel { get; set; }
        public bool ItemsHasOptionLabel { get; set; } = true;
        public Type ItemsServiceType { get; set; }
        public string ItemsServiceMethod { get; set; }
        public string[] ItemsServiceMethodParams { get; set; }

        public bool AjaxSelect { get; set; }
        public string AjaxSelectUrl { get; set; }
        public string AjaxSelectMethod { get; set; }
        public string[] AjaxSelectParams { get; set; }
        public string[] AjaxSelectParamsName { get; set; }

        public bool? _DataTableReload { get; private set; } = null;
        public bool DataTableReload { set => _DataTableReload = value; get => _DataTableReload ?? false; }
    }
}
