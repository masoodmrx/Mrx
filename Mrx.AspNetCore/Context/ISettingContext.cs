namespace Mrx.AspNetCore.Context
{
    public interface ISettingContext
    {
        public DbSet<Setting> Setting { get; set; }
    }
}
