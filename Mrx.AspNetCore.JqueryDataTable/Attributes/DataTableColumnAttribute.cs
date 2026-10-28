namespace Mrx.AspNetCore.JqueryDataTable
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class DataTableColumnAttribute : MetaDataAttribute
    {
        public DataTableColumnAttribute(string className = null)
        {
            ClassName = className;
        }

        private string className;

        public string ClassName
        {
            get
            {
                string export = "";
                if (Export)
                {
                    export = " export";
                }

                return className + export;
            }
            private set => className = value;
        }
        public bool Export { get; set; } = true;
    }
}
