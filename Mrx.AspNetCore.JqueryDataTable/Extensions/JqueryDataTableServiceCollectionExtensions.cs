namespace Mrx.AspNetCore.JqueryDataTable
{
    public static class JqueryDataTableServiceCollectionExtensions
    {
        public static IServiceCollection AddJqueryDataTable(this IServiceCollection services, Action<JqueryDataTableOptions> options)
        {
            services.Configure(options);
            services.AddScoped<JqueryDataTableService>();
            return services;
        }
    }
}
