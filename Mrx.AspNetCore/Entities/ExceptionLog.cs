namespace Mrx.AspNetCore.Entities
{
    public class ExceptionLog
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public string HelpLink { get; set; }
        public string Data { get; set; }
        public string Type { get; set; }

        public int? VisitorId { get; set; }
        public Visitor Visitor { get; set; }
    }
}
