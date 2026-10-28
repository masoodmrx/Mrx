namespace Mrx.AspNetCore.Extensions
{
    public static class PropertyBuilderExtensions
    {
        public static string Domain { set; get; }
        public static PropertyBuilder<TProperty> HasComputedColumnSqlForFileServer<TProperty>(this PropertyBuilder<TProperty> propertyBuilder, string directoryName = null, string defaultFileName = null, string domain = null/*, PropertyBuilder<TProperty> propertyFileName = null*/)
        {
            domain = domain ?? Domain;
            var attr = propertyBuilder?.Metadata?.PropertyInfo?.GetAttribute<FileServerAttribute>();
            var _propertyName = attr?.PropertyName ?? propertyBuilder?.Metadata?.PropertyInfo?.Name?.Replace("Url", "");
            var _directoryName = directoryName ?? attr?.DirectoryName ?? $"/{propertyBuilder?.Metadata?.DeclaringEntityType?.ClrType?.Name}/{_propertyName}";
            var _defaultFileName = defaultFileName ?? attr?.DefaultFileName;
            var path = $"{domain}{_directoryName}/";
            if (_defaultFileName is null) return propertyBuilder.HasComputedColumnSql($"IIF([{_propertyName}] IS NULL, NULL, N'{path}' + [{_propertyName}])");
            return propertyBuilder.HasComputedColumnSql($"N'{path}' + ISNULL([{_propertyName}], '{_defaultFileName}')");
        }
    }
}