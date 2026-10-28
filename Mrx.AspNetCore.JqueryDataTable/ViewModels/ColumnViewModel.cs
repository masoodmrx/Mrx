namespace Mrx.AspNetCore.JqueryDataTable
{
    public class ColumnViewModel
    {
        [JsonProperty("data")]
        public string Name { get; set; }
        [JsonProperty("visible")]
        public bool Visible { get; set; }
        [JsonProperty("sortable")]
        public bool Sortable { get; set; }
        [JsonProperty("searchable")]
        public bool Searchable { get; set; }
        [JsonProperty("render")]
        public string Render { get; set; }
        [JsonProperty("className")]
        public string ClassName { get; set; }
        public List<Dictionary<string, object>> Cases { get; set; }
    }
}
