namespace Mrx.Common.Extensions
{
    public static class LinqExtensions
    {

        public static bool IsNull<Entity>(this IEnumerable<Entity> source)
        {
            return source == null || !source.Any();
        }

        public static bool IsNotNull<Entity>(this IEnumerable<Entity> source)
        {
            return source != null && source.Any();
        }

        public static bool IsNull(this IEnumerable source)
        {
            return source == null || !source.Any();
        }

        public static bool IsNotNull(this IEnumerable source)
        {
            return source != null && source.Any();
        }

        public static bool Any(this IEnumerable source)
        {
            foreach (object item in source)
                return true;
            return false;
        }
        public static T GetNext<T>(this IEnumerable<T> list, T current)
        {
            try
            {
                return list.SkipWhile(x => !x.Equals(current)).Skip(1).First();
            }
            catch
            {
                return default(T);
            }
        }
        public static T GetNext<T>(this IEnumerable<T> list, Func<T, bool> predicate)
        {
            try
            {
                var current = list.FirstOrDefault(predicate);
                return list.GetNext(current);
            }
            catch
            {
                return default(T);
            }
        }
        public static T GetPrevious<T>(this IEnumerable<T> list, T current)
        {
            try
            {
                return list.TakeWhile(x => !x.Equals(current)).Last();
            }
            catch
            {
                return default(T);
            }
        }
        public static T GetPrevious<T>(this IEnumerable<T> list, Func<T, bool> predicate)
        {
            try
            {
                var current = list.FirstOrDefault(predicate);
                return list.GetPrevious(current);
            }
            catch
            {
                return default(T);
            }
        }
        public static IQueryable<TSource> Paging<TSource>(this IQueryable<TSource> source, int pageNumber, int rowsCount)
        {
            if (pageNumber == 0)
                return source;
            return source.Skip((pageNumber - 1) * rowsCount).Take(rowsCount);
        }
        public static IQueryable<TSource> Paging<TSource>(this IQueryable<TSource> source, int pageNumber, int rowsCount, out int pagesCount, out int totalCount)
        {
            totalCount = source.Count();
            pagesCount = pageNumber == 0 ? 1 : (int)Math.Ceiling((double)totalCount / rowsCount);
            return source.Paging(pageNumber, rowsCount);
        }
        public static void RemoveRange<T>(this List<T> entities, Func<T, bool> predicate)
        {
            entities.RemoveRange(entities.Where(predicate).ToList());
        }
        public static void RemoveRange<T>(this List<T> entities, List<T> entities2)
        {
            if (entities2.IsNotNull())
                foreach (var item in entities2)
                    entities.Remove(item);
        }
        public static async Task<IEnumerable<TSource>> WhenAllAsync<TSource>(this IEnumerable<Task<TSource>> source)
        {
            return await Task.WhenAll(source);
        }
        public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> source, Func<TSource, int, TSource> func)
        {
            for (int i = 0; i < source.Count(); i++)
                yield return func(source.ElementAt(i), i);
        }
    }
}
