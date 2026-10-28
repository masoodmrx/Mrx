namespace Mrx.AspNetCore.Entities
{
    public class TableColumnLog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Before { get; set; }
        public string After { get; set; }

        public int LogId { get; set; }
        public TableLog Log { get; set; }
    }
}
