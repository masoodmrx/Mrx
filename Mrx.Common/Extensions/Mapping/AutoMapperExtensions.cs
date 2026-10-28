namespace Mrx.Common.Extensions
{
    public static class AutoMapperExtensions
    {
        //public static T GetService<T>(this ResolutionContext context)
        //{
        //    return default;
        //    //return (T)context.Options.ServiceCtor(typeof(T));
        //}
        public static TDestination Map<TSource, TDestination>(this TSource source, IMapper mapper)
        {
            return mapper.Map<TSource, TDestination>(source);
        }
        public static IEnumerable<TDestination> ProjectTo<TSource, TDestination>(this IEnumerable<TSource> source, IMapper mapper)
        {
            return mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source);
        }
        public static IEnumerable<TDestination> ProjectTo<TSource, TDestination>(this TSource[] source, IMapper mapper)
        {
            return mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source);
        }
    }
}