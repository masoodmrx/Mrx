namespace Mrx.Common.Extensions
{
    public static class MrxServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, params Assembly[] assemblies)
        {
            foreach (Assembly assembly in assemblies)
            {
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
            var _profiles = new List<Type> { typeof(MappingProfile) };
            if (profiles.IsNotNull()) _profiles.AddRange(profiles.ToList());
            foreach (var profile in _profiles) services.AddSingleton(profile);
            services.Configure<AutoMapperOptions>(o =>
            {
                if (assemblies.IsNotNull())
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
