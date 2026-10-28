namespace Mrx.AspNetCore.Services
{
    public class LogService
    {
        private readonly VisitorService visitorService;
        private readonly DbContext db;

        public LogService(
            IServiceProvider serviceProvider,
            IOptions<SiteReportOptions> options,
            IOptions<SiteReportContextOptions> contextOptions,
            VisitorService visitorService)
        {
            this.visitorService = visitorService;
            db = contextOptions.Value.ContextType == null ? null : serviceProvider.GetService(contextOptions.Value.ContextType) as DbContext;
        }
        public async Task SaveLogAsync(DbContext context)
        {
            try
            {
                List<TableLog> logs = GetLogs(context);
                await db.AddRangeAsync(logs);
                await db.SaveChangesAsync();
            }
            catch { }
        }
        public void SaveLog(DbContext context)
        {
            try
            {
                List<TableLog> logs = GetLogs(context);
                db.AddRange(logs);
                db.SaveChanges();
            }
            catch { }
        }
        public List<TableLog> GetLogs(DbContext db)
        {
            return db.ChangeTracker.Entries()
                  .Where(t => t.Entity.GetType().GetCustomAttributes<IgnoreLogAttribute>().Count() == 0 && (t.State == EntityState.Modified || t.State == EntityState.Added || t.State == EntityState.Deleted))
                  .Select(t => new TableLog
                  {
                      Visitor = visitorService.Visitor,
                      Type = t.State.ToString(),
                      RowId = t.Entity.GetType().GetPropertyValue("Id").ConvertToString(),
                      Name = t.Entity.GetType().Name,
                      TableColumnLogs = t.Properties
                      .Select(p => new TableColumnLog { Name = p.Metadata.Name, Before = p.OriginalValue.ConvertToString(), After = p.CurrentValue.ConvertToString() })
                      .ToList()
                  }).ToList();
        }
    }
}
