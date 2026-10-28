namespace Mrx.AspNetCore.Entities
{
    public class QueryString
    {
        public QueryString(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public int VisitorId { get; set; }
        public Visitor Visitor { get; set; }
    }
}
