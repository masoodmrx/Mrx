namespace Mrx.AspNetCore.Options
{
    public class SiteReportOptions
    {
        public bool NotificationByEmail { get; set; } = false;
        public string PageNotFoundPath { get; set; }
        public string InternalServerErrorPath { get; set; }
        public string EmailTitle { get; set; }
        public string[] Emails { get; set; }
    }
}
