using ValidationAttributeAdapterProvider = Mrx.AspNetCore.Validation.Providers.ValidationAttributeAdapterProvider;

namespace Mrx.AspNetCore.Validation.Helpers.Extensions
{
    public static class ValidationServiceCollectionExtensions
    {
        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddSingleton<IValidationAttributeAdapterProvider, ValidationAttributeAdapterProvider>();
            services
                .AddOptions<ValidationOptions>()
                .BindConfiguration("Validation");
            services.AddControllers(options => options.Filters.Add(typeof(ModelStateActionFilterAttribute)));
            return services;
        }
        public static IServiceCollection AddValidation(this IServiceCollection services, Action<ValidationOptions> options)
        {
            services.AddSingleton<IValidationAttributeAdapterProvider, ValidationAttributeAdapterProvider>();
            services.Configure(options);
            services.AddControllers(options => options.Filters.Add(typeof(ModelStateActionFilterAttribute)));
            return services;
        }
    }
}
