using System.Linq.Expressions;

namespace Mrx.Common.Extensions
{
    public static class LinqExtensions
    {

        /// <summary>
        /// Converts a sequence of scalar values or public properties into a <see cref="DataTable"/>.
        /// </summary>
        public static DataTable ToDataTable<T>(this IEnumerable<T> source, string valueColumnName = "Value")
        {
            ArgumentNullException.ThrowIfNull(source);

            var table = new DataTable();
            var itemType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
            if (itemType.IsValueType || itemType == typeof(string))
            {
                var columnType = itemType.IsEnum ? Enum.GetUnderlyingType(itemType) : itemType;
                table.Columns.Add(valueColumnName, columnType);

                foreach (var item in source)
                {
                    var row = table.NewRow();
                    row[0] = ToDataColumnValue(item);
                    table.Rows.Add(row);
                }

                return table;
            }

            var properties = TypeDescriptor.GetProperties(typeof(T))
                .Cast<PropertyDescriptor>()
                .Where(property => property.IsBrowsable)
                .ToArray();

            foreach (var property in properties)
            {
                var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                var columnType = propertyType.IsEnum ? Enum.GetUnderlyingType(propertyType) : propertyType;
                table.Columns.Add(property.Name, columnType);
            }

            foreach (var item in source)
            {
                var row = table.NewRow();
                foreach (var property in properties)
                {
                    row[property.Name] = ToDataColumnValue(item is null ? null : property.GetValue(item));
                }

                table.Rows.Add(row);
            }

            return table;
        }

        private static object ToDataColumnValue(object? value)
        {
            if (value is null)
            {
                return DBNull.Value;
            }

            var valueType = value.GetType();
            return valueType.IsEnum
                ? Convert.ChangeType(value, Enum.GetUnderlyingType(valueType))
                : value;
        }

        /// <summary>
        /// Orders a query by a public instance property without compiling the selector,
        /// so query providers such as Entity Framework can translate it.
        /// </summary>
        public static IOrderedQueryable<TSource> OrderByProperty<TSource>(
            this IQueryable<TSource> source,
            string propertyName,
            bool descending = false)
            => ApplyPropertyOrder(source, propertyName, descending ? nameof(Queryable.OrderByDescending) : nameof(Queryable.OrderBy));

        /// <summary>
        /// Adds a property ordering to an already ordered query.
        /// </summary>
        public static IOrderedQueryable<TSource> ThenByProperty<TSource>(
            this IOrderedQueryable<TSource> source,
            string propertyName,
            bool descending = false)
            => ApplyPropertyOrder(source, propertyName, descending ? nameof(Queryable.ThenByDescending) : nameof(Queryable.ThenBy));

        private static IOrderedQueryable<TSource> ApplyPropertyOrder<TSource>(
            IQueryable<TSource> source,
            string propertyName,
            string methodName)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);

            var property = typeof(TSource).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
            if (property is null || property.GetMethod is null || property.GetIndexParameters().Length != 0)
            {
                throw new ArgumentException($"Public readable property '{propertyName}' was not found on '{typeof(TSource).Name}'.", nameof(propertyName));
            }

            var parameter = Expression.Parameter(typeof(TSource), "item");
            var selector = Expression.Lambda(Expression.Property(parameter, property), parameter);
            var queryMethod = typeof(Queryable).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Single(method => method.Name == methodName
                    && method.IsGenericMethodDefinition
                    && method.GetGenericArguments().Length == 2
                    && method.GetParameters().Length == 2);

            var orderedQuery = queryMethod
                .MakeGenericMethod(typeof(TSource), property.PropertyType)
                .Invoke(null, [source, selector]);

            return (IOrderedQueryable<TSource>)orderedQuery!;
        }

        public static bool IsNullOrEmpty<Entity>(this IEnumerable<Entity>? source)
        {
            return source == null || !source.Any();
        }

        public static bool IsNotNullOrEmpty<Entity>(this IEnumerable<Entity>? source)
        {
            return source != null && source.Any();
        }

        public static bool IsNullOrEmpty(this IEnumerable? source)
        {
            return source == null || !source.Any();
        }

        public static bool IsNotNullOrEmpty(this IEnumerable? source)
        {
            return source != null && source.Any();
        }

        [Obsolete("Use IsNullOrEmpty() instead. This method also returns true for an empty collection.")]
        public static bool IsNull<Entity>(this IEnumerable<Entity>? source) => source.IsNullOrEmpty();

        [Obsolete("Use IsNotNullOrEmpty() instead. This method returns true only when the collection contains an item.")]
        public static bool IsNotNull<Entity>(this IEnumerable<Entity>? source) => source.IsNotNullOrEmpty();

        [Obsolete("Use IsNullOrEmpty() instead. This method also returns true for an empty collection.")]
        public static bool IsNull(this IEnumerable? source) => source.IsNullOrEmpty();

        [Obsolete("Use IsNotNullOrEmpty() instead. This method returns true only when the collection contains an item.")]
        public static bool IsNotNull(this IEnumerable? source) => source.IsNotNullOrEmpty();

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
                ArgumentNullException.ThrowIfNull(list);
                var comparer = EqualityComparer<T>.Default;
                var currentFound = false;
                foreach (var item in list)
                {
                    if (currentFound)
                        return item;
                    if (comparer.Equals(item, current))
                        currentFound = true;
                }
                return default;
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
                ArgumentNullException.ThrowIfNull(list);
                ArgumentNullException.ThrowIfNull(predicate);
                var currentFound = false;
                foreach (var item in list)
                {
                    if (currentFound)
                        return item;
                    if (predicate(item))
                        currentFound = true;
                }
                return default;
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
                ArgumentNullException.ThrowIfNull(list);
                var comparer = EqualityComparer<T>.Default;
                var previous = default(T);
                foreach (var item in list)
                {
                    if (comparer.Equals(item, current))
                        return previous;
                    previous = item;
                }
                return default;
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
                ArgumentNullException.ThrowIfNull(list);
                ArgumentNullException.ThrowIfNull(predicate);
                var previous = default(T);
                foreach (var item in list)
                {
                    if (predicate(item))
                        return previous;
                    previous = item;
                }
                return default;
            }
            catch
            {
                return default(T);
            }
        }
        public static IQueryable<TSource> Paging<TSource>(this IQueryable<TSource> source, int pageNumber, int rowsCount)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentOutOfRangeException.ThrowIfNegative(pageNumber);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rowsCount);
            if (pageNumber == 0)
                return source;
            return source.Skip((pageNumber - 1) * rowsCount).Take(rowsCount);
        }
        public static IQueryable<TSource> Paging<TSource>(this IQueryable<TSource> source, int pageNumber, int rowsCount, out int pagesCount, out int totalCount)
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentOutOfRangeException.ThrowIfNegative(pageNumber);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rowsCount);
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
            if (entities2.IsNotNullOrEmpty())
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
