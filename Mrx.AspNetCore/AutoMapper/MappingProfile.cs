namespace Mrx.AspNetCore.AutoMapper
{
    [Service]
    public class MappingProfile : Profile
    {
        public MappingProfile(IOptions<AutoMapperOptions> options)
        {
            foreach (var assembly in options.Value.Assemblies)
            {
                foreach (var type in assembly.GetTypes().Where(p => p.GetInterfaces().Contains(typeof(IAutoMapper))))
                {
                    object instance = Activator.CreateInstance(type);
                    if (instance is IAutoMapper autoMapper)
                        autoMapper.Mapping(this);
                }
            }
        }
    }
}
