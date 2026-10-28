namespace Mrx.AspNetCore.Extensions
{
    public static class EntityFrameworkExtensions
    {
        public static int EnableIdentityInsert<T>(this DbContext context) => EnableIdentityInsert(context, typeof(T));
        public async static Task<int> EnableIdentityInsertAsync<T>(this DbContext context) => await EnableIdentityInsertAsync(context, typeof(T));
        public static int EnableIdentityInsert(this DbContext context, Type entityType) => SetIdentityInsert(context, entityType, true);
        public async static Task<int> EnableIdentityInsertAsync(this DbContext context, Type entityType) => await SetIdentityInsertAsync(context, entityType, true);

        public static int DisableIdentityInsert<T>(this DbContext context) => DisableIdentityInsert(context, typeof(T));
        public async static Task<int> DisableIdentityInsertAsync<T>(this DbContext context) => await DisableIdentityInsertAsync(context, typeof(T));
        public static int DisableIdentityInsert(this DbContext context, Type entityType) => SetIdentityInsert(context, entityType, false);
        public async static Task<int> DisableIdentityInsertAsync(this DbContext context, Type entityType) => await SetIdentityInsertAsync(context, entityType, false);

        private static int SetIdentityInsert<T>(DbContext context, bool enable)
        {
            return SetIdentityInsert(context, typeof(T), enable);
        }
        private static int SetIdentityInsert(DbContext context, Type entityType, bool enable)
        {
            var entity = context.Model.FindEntityType(entityType);
            return context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT {entity.GetSchema()}.{entity.GetTableName()} {(enable ? "ON" : "OFF")};");
        }
        private async static Task<int> SetIdentityInsertAsync<T>(DbContext context, bool enable)
        {
            return await SetIdentityInsertAsync(context, typeof(T), enable);
        }
        private async static Task<int> SetIdentityInsertAsync(DbContext context, Type entityType, bool enable)
        {
            var entity = context.Model.FindEntityType(entityType);
            return await context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {entity.GetSchema()}.{entity.GetTableName()} {(enable ? "ON" : "OFF")};");
        }

        public static int SaveChangesWithIdentityInsert<T>(this DbContext context)
        {
            return context.SaveChangesWithIdentityInsert(typeof(T));
        }
        public async static Task<int> SaveChangesWithIdentityInsertAsync<T>(this DbContext context)
        {
            return await context.SaveChangesWithIdentityInsertAsync(new Type[] { typeof(T) });
        }
        public static int SaveChangesWithIdentityInsert(this DbContext context, params Type[] entityTypes)
        {
            using var transaction = context.Database.BeginTransaction();
            foreach (var entityType in entityTypes) context.EnableIdentityInsert(entityType);
            int result = context.SaveChanges();
            foreach (var entityType in entityTypes) context.DisableIdentityInsert(entityType);
            transaction.Commit();
            return result;
        }
        public async static Task<int> SaveChangesWithIdentityInsertAsync(this DbContext context, params Type[] entityTypes)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            foreach (var entityType in entityTypes) await context.EnableIdentityInsertAsync(entityType);
            int result = await context.SaveChangesAsync();
            foreach (var entityType in entityTypes) await context.EnableIdentityInsertAsync(entityType);
            await transaction.CommitAsync();
            return result;
        }

        public static void RemoveRange<TEntity>(this DbSet<TEntity> entities, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            entities.RemoveRange(entities.Where(predicate));
        }
        public static void RemoveAll<TEntity>(this DbSet<TEntity> entities) where TEntity : class
        {
            entities.RemoveRange(entities);
        }
    }
}
