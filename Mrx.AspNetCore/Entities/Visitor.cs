namespace Mrx.AspNetCore.Entities
{
    public class Visitor
    {
        public int Id { get; set; }
        public DateTime DateRegister { get; set; }
        [MaxLength(10)]
        public string Method { get; set; }
        [MaxLength(50)]
        public string Ip { get; set; }
        public string Path { get; set; }
        [MaxLength(50)]
        public string UserId { get; set; }
        public string UserAgent { get; set; }
        [MaxLength(50)]
        public string Device { get; set; }
        [MaxLength(50)]
        public string PlatformName { get; set; }
        [MaxLength(50)]
        public string PlatformProcessor { get; set; }
        [MaxLength(50)]
        public string EngineName { get; set; }
        [MaxLength(50)]
        public string EngineVersion { get; set; }
        [MaxLength(50)]
        public string BrowserName { get; set; }
        [MaxLength(50)]
        public string BrowserVersion { get; set; }
        public bool IsCrawler { get; set; }
        [MaxLength(50)]
        public string CrawlerName { get; set; }
        [MaxLength(50)]
        public string CrawlerVersion { get; set; }

        public List<TableLog> TableLogs { get; set; }
        public List<ExceptionLog> ExceptionLogs { get; set; }
        public List<QueryString> QueryStrings { get; set; }
    }
}
