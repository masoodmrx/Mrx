using QueryString = Mrx.AspNetCore.Entities.QueryString;

namespace Mrx.AspNetCore.Context
{
    public interface ISiteReportContext
    {
        public DbSet<ExceptionLog> ExceptionLog { get; set; }
        public DbSet<QueryString> QueryString { get; set; }
        public DbSet<TableColumnLog> TableColumnLog { get; set; }
        public DbSet<TableLog> TableLog { get; set; }
        public DbSet<Visitor> Visitor { get; set; }
    }
}
