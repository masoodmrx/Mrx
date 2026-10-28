using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Mrx.AspNetCore.Conventions
{
    /// <summary>
    /// Adds a model-only trigger declaration to mapped tables that do not declare one.
    /// This can be used with relational providers whose generated SQL changes when a table
    /// has database triggers. It does not create triggers in the database.
    /// </summary>
    public sealed class BlankTriggerAddingConvention : IModelFinalizingConvention
    {
        public void ProcessModelFinalizing(
            IConventionModelBuilder modelBuilder,
            IConventionContext<IConventionModelBuilder> context)
        {
            foreach (var entityType in modelBuilder.Metadata.GetEntityTypes())
            {
                var table = StoreObjectIdentifier.Create(entityType, StoreObjectType.Table);
                if (table is not null
                    && entityType.GetDeclaredTriggers().All(trigger => trigger.GetDatabaseName(table.Value) is null)
                    && (entityType.BaseType is null
                        || entityType.GetMappingStrategy() != RelationalAnnotationNames.TphMappingStrategy))
                {
                    entityType.Builder.HasTrigger($"{table.Value.Name}_Trigger");
                }

                foreach (var fragment in entityType.GetMappingFragments(StoreObjectType.Table))
                {
                    if (entityType.GetDeclaredTriggers().All(trigger => trigger.GetDatabaseName(fragment.StoreObject) is null))
                    {
                        entityType.Builder.HasTrigger($"{fragment.StoreObject.Name}_Trigger");
                    }
                }
            }
        }
    }
}
