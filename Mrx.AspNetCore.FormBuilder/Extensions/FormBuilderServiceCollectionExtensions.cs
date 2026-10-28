namespace Mrx.AspNetCore.FormBuilder
{
    public static class FormBuilderServiceCollectionExtensions
    {
        public static IServiceCollection AddFormBuilder(this IServiceCollection services, Action<FormBuilderOptions> options)
        {
            services.Configure(options);
            return services;
        }
    }
}
