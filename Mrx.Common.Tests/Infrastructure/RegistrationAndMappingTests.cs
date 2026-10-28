using Mrx.Common.Attributes;
using Mrx.Common.Extensions;
using Mrx.Common.Helpers;
using Mrx.Common.Interfaces;
using Mrx.Common.Options;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Mrx.Common.Tests;

public class RegistrationAndMappingTests
{
    [Fact]
    public void AutoMapperExtensions_MapAndProjectSequences()
    {
        var configuration = new MapperConfiguration(
            cfg => cfg.CreateMap<MappingSource, MappingDestination>(),
            NullLoggerFactory.Instance);
        var mapper = configuration.CreateMapper();
        var source = new MappingSource { Name = "Ada" };

        Assert.Equal("Ada", source.Map<MappingSource, MappingDestination>(mapper).Name);
        Assert.Equal("Grace", new[] { new MappingSource { Name = "Grace" } }.ProjectTo<MappingSource, MappingDestination>(mapper).Single().Name);
        Assert.Equal("Lin", ((IEnumerable<MappingSource>)new[] { new MappingSource { Name = "Lin" } }).ProjectTo<MappingSource, MappingDestination>(mapper).Single().Name);
    }

    [Fact]
    public void AddServices_RegistersAttributedImplementationWithConfiguredLifetimeAndServiceType()
    {
        var services = new ServiceCollection();

        Assert.Same(services, services.AddServices(typeof(AttributedService).Assembly));
        var descriptor = Assert.Single(services, item => item.ServiceType == typeof(ITestService));
        Assert.Equal(typeof(AttributedService), descriptor.ImplementationType);
        Assert.Equal(ServiceLifetime.Singleton, descriptor.Lifetime);
    }

    [Fact]
    public void AddServices_AllowsRepeatedCallsAndRegistersEachDescriptor()
    {
        var services = new ServiceCollection();

        services.AddServices(typeof(AttributedService).Assembly);
        services.AddServices(typeof(AttributedService).Assembly);

        Assert.Equal(2, services.Count(item => item.ServiceType == typeof(ITestService)));
        using var provider = services.BuildServiceProvider();
        Assert.IsType<AttributedService>(provider.GetRequiredService<ITestService>());
        Assert.Equal(2, provider.GetServices<ITestService>().Count());
    }

    [Fact]
    public void AddServices_RejectsNullServicesAndAssemblyInputs()
    {
        Assert.Throws<ArgumentNullException>(() => MrxServiceCollectionExtensions.AddServices(null!, typeof(AttributedService).Assembly));
        Assert.Throws<ArgumentNullException>(() => new ServiceCollection().AddServices((Assembly[])null!));
        Assert.Throws<ArgumentNullException>(() => new ServiceCollection().AddServices(new Assembly[] { null! }));
    }

    [Fact]
    public void AddAutoMapperModels_ResolvesCustomProfileAndMappingImplementations()
    {
        var services = new ServiceCollection();
        services.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory>(NullLoggerFactory.Instance);
        services.AddAutoMapperModels(
            new[] { typeof(RegistrationAndMappingTests).Assembly },
            new[] { typeof(ExplicitMappingProfile) });
        using var provider = services.BuildServiceProvider();
        var mapper = provider.GetRequiredService<IMapper>();

        var result = mapper.Map<MappingSource, MappingDestination>(new MappingSource { Name = "Mapped by profile" });
        Assert.Equal("Mapped by profile", result.Name);

        var discovered = mapper.Map<ConventionSource, ConventionDestination>(new ConventionSource { Value = 12 });
        Assert.Equal(12, discovered.Value);
    }

    [Fact]
    public void AddAutoMapperModels_RejectsInvalidProfileAndNullAssemblyEntries()
    {
        Assert.Throws<ArgumentException>(() => new ServiceCollection().AddAutoMapperModels(profiles: [typeof(string)]));
        Assert.Throws<ArgumentException>(() => new ServiceCollection().AddAutoMapperModels(assemblies: [null!]));
    }

    [Fact]
    public void AddAutoMapperModels_RepeatedRegistrationCanResolveAllRegisteredMappers()
    {
        var services = new ServiceCollection();
        services.AddSingleton<Microsoft.Extensions.Logging.ILoggerFactory>(NullLoggerFactory.Instance);
        services.AddAutoMapperModels(profiles: [typeof(ExplicitMappingProfile)]);
        services.AddAutoMapperModels(profiles: [typeof(ExplicitMappingProfile)]);

        using var provider = services.BuildServiceProvider();
        var mappers = provider.GetServices<IMapper>().ToArray();

        Assert.Equal(2, mappers.Length);
        Assert.Equal("Ada", mappers[0].Map<MappingSource, MappingDestination>(new MappingSource { Name = "Ada" }).Name);
        Assert.Equal("Grace", mappers[1].Map<MappingSource, MappingDestination>(new MappingSource { Name = "Grace" }).Name);
    }

    [Fact]
    public void MappingProfile_UsesOptionsAssembliesToFindIAutoMapperImplementations()
    {
        var options = Microsoft.Extensions.Options.Options.Create(new AutoMapperOptions());
        options.Value.Assemblies.Add(typeof(RegistrationAndMappingTests).Assembly);

        var profile = new Mrx.Common.AutoMapper.MappingProfile(options);
        var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile), NullLoggerFactory.Instance);
        Assert.Equal(5, configuration.CreateMapper().Map<ConventionSource, ConventionDestination>(new ConventionSource { Value = 5 }).Value);
    }

    [Fact]
    public void ServiceActivator_CreatesScopeFromExplicitOrConfiguredProvider()
    {
        var provider = new ServiceCollection().BuildServiceProvider();
        ServiceActivator.Configure(provider);

        using var fromConfiguredProvider = ServiceActivator.GetScope();
        using var fromExplicitProvider = ServiceActivator.GetScope(provider);

        Assert.NotNull(fromConfiguredProvider);
        Assert.NotNull(fromExplicitProvider);
        Assert.NotNull(ServiceActivator.GetScope(null)); // The configured provider remains the fallback.
    }

    [Fact]
    public void MetadataAttributes_ExposeConfiguredValues()
    {
        var fileAttribute = typeof(AttributedMetadata).GetProperty(nameof(AttributedMetadata.File))!.GetCustomAttributes(typeof(FileServerAttribute), false).Cast<FileServerAttribute>().Single();
        Assert.Equal("images", fileAttribute.DirectoryName);
        Assert.Equal("download", fileAttribute.DefaultFileName);
        Assert.Equal(nameof(AttributedMetadata.File), fileAttribute.PropertyName);
        Assert.Equal(ServiceLifetime.Scoped, new ServiceAttribute().ServiceLifetime);
        Assert.Equal(typeof(ITestService), new ServiceAttribute(typeof(ITestService)).ServiceType);
        Assert.Equal("metadata", new DataAttribute("metadata").Value);
        Assert.Equal("home", new IconAttribute("home").Icon);
        Assert.Null(new FileServerAttribute().DirectoryName);
    }

    public interface ITestService { }

    [Service(ServiceLifetime.Singleton, typeof(ITestService))]
    public sealed class AttributedService : ITestService { }

    public sealed class ExplicitMappingProfile : Profile
    {
        public ExplicitMappingProfile() => CreateMap<MappingSource, MappingDestination>();
    }

    public sealed class ConventionSource { public int Value { get; set; } }
    public sealed class ConventionDestination { public int Value { get; set; } }
    public sealed class MappingSource { public string? Name { get; set; } }
    public sealed class MappingDestination { public string? Name { get; set; } }

    public sealed class ConventionMapping : IAutoMapper
    {
        public void Mapping(Profile profile) => profile.CreateMap<ConventionSource, ConventionDestination>();
    }

    public sealed class AttributedMetadata
    {
        [FileServer("images", DefaultFileName = "download", PropertyName = nameof(File))]
        public string? File { get; set; }
    }
}
