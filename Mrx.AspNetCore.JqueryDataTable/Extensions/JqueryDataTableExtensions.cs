namespace Mrx.AspNetCore.JqueryDataTable
{
    public static class JqueryDataTableExtensions
    {
        public static IQueryable<TSource> Sorting<TSource>(this IQueryable<TSource> source, JqueryDataTablesParameters table)
        {
            //string OrderByColumn = table.Order.Any() ? table.Columns[table.Order.FirstOrDefault().Column].Data : null;
            //bool OrderDirection = table.Order.FirstOrDefault()?.Dir == DTOrderDir.DESC;
            //source.OrderBy(p=>p.);
            //return source;
            return SortOptionsProcessor<TSource, TSource>.Apply(source, table);
        }
        public static IQueryable<TSource> Paging<TSource>(this IQueryable<TSource> source, JqueryDataTablesParameters table)
        {
            if (table.Length == 0)
            {
                return source;
            }
            else
            {
                var a = table.Start / table.Length * table.Length;
                return source
                    .Skip(table.Start / table.Length * table.Length)
                    .Take(table.Length);
            }
        }
        public static IQueryable<TSource> Paging<TSource>(this IQueryable<TSource> source, JqueryDataTablesParameters table, out int totalCount)
        {
            totalCount = source.Count();
            return source.Paging(table);
        }
        public static IStoredProcBuilder Paging(this IStoredProcBuilder storedProcBuilder, int pageNumber, int rowsCount, out IOutParam<int> totalCount)
        {
            storedProcBuilder = storedProcBuilder
                .AddParam("PageNumber", pageNumber)
                .AddParam("RowsCount", rowsCount)
                .AddParam("PageCount", out IOutParam<int> PageCount)
                .AddParam("TotalCount", out IOutParam<int> TotalCount);
            totalCount = TotalCount;
            return storedProcBuilder;
        }
        public static IStoredProcBuilder Paging(this IStoredProcBuilder storedProcBuilder, JqueryDataTablesParameters table, out IOutParam<int> totalCount)
        {
            int PageNumber = table.Length <= 0 ? 0 : (table.Start + table.Length) / table.Length;
            int RowsCount = table.Length;
            return storedProcBuilder.Paging(PageNumber, RowsCount, out totalCount);
        }
        public static IStoredProcBuilder Sorting(this IStoredProcBuilder storedProcBuilder, JqueryDataTablesParameters table)
        {
            string OrderByColumn = table.Order.Any() ? table.Columns[table.Order.FirstOrDefault().Column].Data : null;
            bool OrderDirection = table.Order.FirstOrDefault()?.Dir == DTOrderDir.DESC;
            return storedProcBuilder
                .AddParam("OrderByColumn", OrderByColumn)
                .AddParam("OrderDirection", OrderDirection);
        }
        public static IStoredProcBuilder AddParams<TModel>(this IStoredProcBuilder storedProcBuilder, TModel model) where TModel : class, new()
        {
            //IStoredProcBuilder AddParam<T>(string name, T val);
            MethodInfo method1 = storedProcBuilder.GetType().GetMethods()
                .FirstOrDefault(p => p.Name == "AddParam" && p.GetParameters().Count() == 2 && !p.GetParameters()[1].IsOut);
            //IStoredProcBuilder AddParam<T>(string name, out IOutParam<T> outParam);
            MethodInfo method2 = storedProcBuilder.GetType().GetMethods()
                .FirstOrDefault(p => p.Name == "AddParam" && p.GetParameters().Count() == 2 && p.GetParameters()[1].IsOut);
            //IStoredProcBuilder AddParam<T>(string name, out IOutParam<T> outParam, int size = 0, byte precision = 0, byte scale = 0);
            MethodInfo method3 = storedProcBuilder.GetType().GetMethods()
                .FirstOrDefault(p => p.Name == "AddParam" && p.GetParameters().Count() == 5);

            foreach (PropertyInfo property in typeof(TModel).GetProperties())
            {
                if (property.PropertyType.GetTypeNullableType().IsEnum)
                {
                    Type type = Enum.GetUnderlyingType(property.PropertyType.GetTypeNullableType());
                    type = property.GetValue(model) == null ? typeof(Nullable<>).MakeGenericType(type) : type;
                    method1.MakeGenericMethod(type)
                      .Invoke(storedProcBuilder, new[] { property.Name, property.GetValue(model) == null ? null : Convert.ChangeType(property.GetValue(model), type) });
                }
                else
                {
                    if (property.PropertyType.Name == "IOutParam`1")
                    {
                        var attr = property.GetCustomAttribute<StoredProcParamAttribute>();
                        if (attr == null)
                        {
                            object[] parameters = new object[] { property.Name, null };
                            method2.MakeGenericMethod(property.PropertyType.GetGenericArguments()[0])
                              .Invoke(storedProcBuilder, parameters);
                            property.SetValue(model, parameters[1]);
                        }
                        else
                        {
                            object[] parameters = new object[] { property.Name, null, attr.Size, attr.Precision, attr.Scale };
                            method3.MakeGenericMethod(property.PropertyType.GetGenericArguments()[0])
                              .Invoke(storedProcBuilder, parameters);
                            property.SetValue(model, parameters[1]);
                        }
                    }
                    else
                    {
                        method1.MakeGenericMethod(property.PropertyType)
                          .Invoke(storedProcBuilder, new[] { property.Name, property.GetValue(model) });
                    }
                }
            }
            return storedProcBuilder;
        }
        public static JqueryDataTablesResult<T> GetResult<T>(this JqueryDataTablesParameters table, JqueryDataTablesPagedResults<T> results)
        {
            try
            {
                return new JqueryDataTablesResult<T>
                {
                    Draw = table.Draw,
                    Data = results.Items,
                    RecordsFiltered = results.TotalSize,
                    RecordsTotal = results.TotalSize
                };
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return null;
            }
        }
        public static JqueryDataTablesResult<T> GetResult<T>(this IEnumerable<T> items, JqueryDataTablesParameters table, int? totalSize = null)
        {
            return table.GetResult(new JqueryDataTablesPagedResults<T>
            {
                Items = items,
                TotalSize = totalSize ?? items.Count()
            });
        }
    }
}