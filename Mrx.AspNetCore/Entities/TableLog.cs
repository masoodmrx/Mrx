namespace Mrx.AspNetCore.Entities
{
    public class TableLog
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string RowId { get; set; }
        [MaxLength(30)]
        public string Type { get; set; }

        public List<TableColumnLog> TableColumnLogs { get; set; }
        public int? VisitorId { get; set; }
        public Visitor Visitor { get; set; }
    }
}
