using QueryString = Mrx.AspNetCore.Entities.QueryString;

namespace Mrx.AspNetCore.Services
{
    public class VisitorService
    {
        public Visitor Visitor { get; private set; }

        private readonly SiteReportOptions options;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IDetectionService detectionService;
        private readonly EmailService emailService;
        private readonly IViewRenderService viewRenderService;
        private readonly DbContext db;

        public VisitorService(
            IOptions<SiteReportOptions> options,
            IOptions<SiteReportContextOptions> contextOptions,
            IHttpContextAccessor httpContextAccessor,
            IServiceProvider serviceProvider,
            IDetectionService detectionService,
            EmailService emailService,
            IViewRenderService viewRenderService)
        {
            this.options = options.Value;
            this.httpContextAccessor = httpContextAccessor;
            this.detectionService = detectionService;
            this.emailService = emailService;
            this.viewRenderService = viewRenderService;
            db = contextOptions.Value.ContextType == null ? null : serviceProvider.GetService(contextOptions.Value.ContextType) as DbContext;

        }
        public async Task<Visitor> VisitAsync()
        {
            Visitor = new Visitor
            {
                DateRegister = DateTime.Now,
                Method = httpContextAccessor.HttpContext.Request.Method,
                Ip = httpContextAccessor.GetIp(),
                Path = httpContextAccessor.HttpContext.Request.GetDisplayUrl(),
                UserId = httpContextAccessor.GetUserId(),
                UserAgent = detectionService.UserAgent.ToString(),
                Device = detectionService.Device.Type.ToString(),
                PlatformName = detectionService.Platform.Name.ToString(),
                PlatformProcessor = detectionService.Platform.Processor.ToString(),
                EngineName = detectionService.Engine.Name.ToString(),
                //EngineVersion = detectionService.Engine.Version.ToString(),
                BrowserName = detectionService.Browser.Name.ToString(),
                BrowserVersion = detectionService.Browser.Version.ToString(),
                IsCrawler = detectionService.Crawler.IsCrawler,
                CrawlerName = detectionService.Crawler.IsCrawler ? detectionService.Crawler.Name.ToString() : null,
                CrawlerVersion = detectionService.Crawler.IsCrawler ? detectionService.Crawler.Version.ToString() : null,
                TableLogs = new List<TableLog>(),
                ExceptionLogs = new List<ExceptionLog>(),
                QueryStrings = new List<QueryString>()
            };
            try
            {
                if (httpContextAccessor.HttpContext.Request.Query.Any())
                {
                    foreach (KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues> kv in httpContextAccessor.HttpContext.Request.Query.OrderBy(kv => kv.Key))
                    {
                        foreach (string v in kv.Value)
                        {
                            Visitor.QueryStrings.Add(new QueryString(kv.Key, v));
                        }
                    }
                }
            }
            catch { }
            try
            {
                if (httpContextAccessor.HttpContext.Request.Form.Any())
                {
                    foreach (KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues> kv in httpContextAccessor.HttpContext.Request.Form.OrderBy(kv => kv.Key))
                    {
                        foreach (string v in kv.Value)
                        {
                            Visitor.QueryStrings.Add(new QueryString(kv.Key, v));
                        }
                    }
                }
                //Visitor.QueryStrings.Concat(formCollection.Select(p => new QueryString(p.Key, p.Value)).ToList());
            }
            catch { }
            if (db != null)
            {
                await db.AddAsync(Visitor);
                await db.SaveChangesAsync();
            }
            return Visitor;
        }
        public async Task RegisterExceptionAsync(Exception exception)
        {
            if (exception != null)
            {
                do
                {
                    ExceptionLog log = new ExceptionLog
                    {
                        Visitor = Visitor,
                        Type = exception.GetType().Name,
                        Message = exception.Message,
                        StackTrace = exception.StackTrace,
                        Data = exception.Data.ToJson(),
                        HelpLink = exception.HelpLink,
                        Source = exception.Source
                    };
                    if (db != null)
                    {
                        await db.AddAsync(log);
                    }
                    if (options.NotificationByEmail)
                    {
                        try
                        {
                            string html = await viewRenderService.RenderToStringAsync("_Exception", log);
                            emailService.Send($"{options.EmailTitle} Exception : {log.Type}", html, options.Emails);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    exception = exception.InnerException;
                } while (exception != null);
                if (db != null)
                {
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
