namespace Mrx.AspNetCore.Extensions
{
    //public class NameMeJohnAction : IMappingAction<object, object>
    //{
    //    //private readonly IHttpContextAccessor httpContextAccessor;

    //    //public NameMeJohnAction(IHttpContextAccessor httpContextAccessor)
    //    //{
    //    //    this.httpContextAccessor = httpContextAccessor;
    //    //}
    //    public void Process(object source, object destination, ResolutionContext context)
    //    {
    //        //var a = httpContextAccessor.GetUserId();
    //    }
    //}
    public static class MrxServiceCollectionExtensions
    {
        public static IServiceCollection AddMrx(this IServiceCollection services)
        {
            //var viewAssembly = typeof(MrxServiceCollectionExtensions).GetTypeInfo().Assembly;
            //var fileProvider = new EmbeddedFileProvider(viewAssembly);
            //services.Configure<RazorViewEngineOptions>(o => o.FileProviders.Add(fileProvider));

            //services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            //{
            //    options.FileProviders.Add(
            //    new EmbeddedFileProvider(typeof(MrxServiceCollectionExtensions).GetTypeInfo().Assembly));
            //});

            services.AddServices(typeof(MrxServiceCollectionExtensions).Assembly);
            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            return services;
        }
        public static IServiceCollection AddViewRenderService(this IServiceCollection services)
        {
            return services.AddScoped<IViewRenderService, ViewRenderService>();
        }
        //public static IServiceCollection AddServices(this IServiceCollection services, params Assembly[] assemblies)
        //{
        //    foreach (Assembly assembly in assemblies)
        //    {
        //        foreach (Type service in assembly.GetTypes().Where(p => p.GetCustomAttribute<ServiceAttribute>() != null))
        //        {
        //            ServiceAttribute attr = service.GetCustomAttribute<ServiceAttribute>();
        //            services.Add(new ServiceDescriptor(attr.ServiceType ?? service, service, attr.ServiceLifetime));
        //        }
        //    }
        //    return services;
        //}
        //public static IServiceCollection AddAutoMapperModels(this IServiceCollection services, Assembly[] assemblies = null, Type[] profiles = null)
        //{
        //    var _profiles = new List<Type> { typeof(MappingProfile) };
        //    if (profiles.IsNotNull()) _profiles.AddRange(profiles.ToList());
        //    foreach (var profile in _profiles) services.AddScoped(profile);
        //    services.Configure<AutoMapperOptions>(o =>
        //    {
        //        if (assemblies.IsNotNull())
        //            o.Assemblies.AddRange(assemblies);
        //    });
        //    //services.AddAutoMapper(typeof(MappingProfile));
        //    services.AddSingleton(provider => new MapperConfiguration(cfg =>
        //    {
        //        //cfg.CreateMap<object, object>()
        //        //.AfterMap<NameMeJohnAction>();
        //        cfg.ConstructServicesUsing((type) =>
        //        {
        //            return services.BuildServiceProvider().CreateScope().ServiceProvider.GetService(type);
        //            //var httpRequestMessage = HttpContext.Current.Items["MS_HttpRequestMessage"] as HttpRequestMessage;
        //            //var currentDependencyScope = httpRequestMessage.GetDependencyScope();
        //            //return currentDependencyScope.GetService(type);
        //        });
        //        cfg.AddProfiles(_profiles.Select(profile => (Profile)services.BuildServiceProvider().GetService(profile)));
        //        //cfg.AddProfile(services.BuildServiceProvider().CreateScope().ServiceProvider.GetService<MappingProfile>());
        //        //cfg.AddProfile(new MappingProfile(provider));
        //    }).CreateMapper());
        //    return services;
        //}
        public static IServiceCollection AddSiteReport(this IServiceCollection services)
        {
            services.AddDetection();
            services.AddScoped<VisitorService>();
            services.AddScoped<LogService>();
            services
                .AddOptions<SiteReportOptions>()
                .BindConfiguration("SiteReport");
            return services;
        }
        public static IServiceCollection AddSiteReport(this IServiceCollection services, Action<SiteReportOptions> options)
        {
            services.AddDetection();
            services.AddScoped<VisitorService>();
            services.AddScoped<LogService>();
            return services.Configure(options);
        }
        public static IServiceCollection AddSiteReportContext<TContext>(this IServiceCollection services)
        where TContext : class, ISiteReportContext
        {
            return services.Configure<SiteReportContextOptions>(o => o.ContextType = typeof(TContext));
        }
        public static void UseSiteReport(this IApplicationBuilder app)
        {
            app.UseMiddleware<VisitorMiddleware>();
        }
        public static IServiceCollection AddSetting<TContext>(this IServiceCollection services)
        where TContext : class, ISettingContext
        {
            services.Configure<SettingOptions>(o =>
            {
                o.ContextType = typeof(TContext);
            });
            return services;
        }
        public static IServiceCollection AddFtp(this IServiceCollection services)
        {
            services.AddScoped<FtpService>();
            services
                .AddOptions<FtpOptions>()
                .BindConfiguration("Ftp");
            return services;
        }
        public static IServiceCollection AddFtp(this IServiceCollection services, Action<FtpOptions> options)
        {
            services.AddScoped<FtpService>();
            return services.Configure(options);
        }
        //public static IServiceCollection AddFtpAccount(this IServiceCollection services, FtpAccountOptions ftpAccount)
        //{
        //    services.AddScoped<FtpService>();
        //    return services.AddFtp(ftp => ftp.Accounts.Add(ftpAccount));
        //}
        public static IServiceCollection AddEmail(this IServiceCollection services)
        {
            services.AddScoped<EmailService>();
            services
                .AddOptions<EmailOptions>()
                .BindConfiguration("Email");
            return services;
        }
        public static IServiceCollection AddEmail(this IServiceCollection services, Action<EmailOptions> options)
        {
            services.AddScoped<EmailService>();
            return services.Configure(options);
        }
        //public static IServiceCollection AddEmailService(this IServiceCollection services, Action<EmailOptions> options)
        //{
        //    services.AddScoped<EmailService>();
        //    return services.Configure(options);
        //}
    }
}
