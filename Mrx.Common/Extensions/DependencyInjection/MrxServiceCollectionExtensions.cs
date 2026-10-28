namespace Mrx.Common.Extensions
{
    public static class MrxServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, params Assembly[] assemblies)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(assemblies);
            foreach (Assembly assembly in assemblies)
            {
                ArgumentNullException.ThrowIfNull(assembly);
                foreach (Type service in assembly.GetTypes().Where(p => p.GetCustomAttribute<ServiceAttribute>() != null))
                {
                    ServiceAttribute attr = service.GetCustomAttribute<ServiceAttribute>();
                    services.Add(new ServiceDescriptor(attr.ServiceType ?? service, service, attr.ServiceLifetime));
                }
            }
            return services;
        }
        public static IServiceCollection AddAutoMapperModels(this IServiceCollection services, Assembly[] assemblies = null, Type[] profiles = null)
        {
            ArgumentNullException.ThrowIfNull(services);
            var _profiles = new List<Type> { typeof(MappingProfile) };
            if (profiles != null)
            {
                if (profiles.Any(profile => profile == null || !typeof(Profile).IsAssignableFrom(profile)))
                    throw new ArgumentException("Every AutoMapper profile type must derive from Profile.", nameof(profiles));
                _profiles.AddRange(profiles);
            }
            if (assemblies?.Any(assembly => assembly == null) == true)
                throw new ArgumentException("Assembly entries cannot be null.", nameof(assemblies));
            foreach (var profile in _profiles) services.AddSingleton(profile);
            services.Configure<AutoMapperOptions>(o =>
            {
                if (assemblies.IsNotNullOrEmpty())
                    o.Assemblies.AddRange(assemblies);
            });
            //services.AddAutoMapper(typeof(MappingProfile));
            services.AddSingleton<IMapper>(provider =>
            {
                var mapperConfiguration = new MapperConfiguration(cfg =>
                {
                    cfg.ConstructServicesUsing(provider.GetService);
                    cfg.AddProfiles(_profiles.Select(profile => (Profile)provider.GetRequiredService(profile)));
                }, provider.GetRequiredService<ILoggerFactory>());

                return mapperConfiguration.CreateMapper(provider.GetService);
            });
            return services;
        }
    }
}
