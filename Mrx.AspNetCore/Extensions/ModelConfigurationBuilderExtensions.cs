namespace Mrx.AspNetCore.Extensions
{
    public static class ModelConfigurationBuilderExtensions
    {
        /// <summary>
        /// Opts this DbContext into <see cref="Conventions.BlankTriggerAddingConvention"/>.
        /// The convention adds model metadata only; it does not create database triggers.
        /// </summary>
        public static ModelConfigurationBuilder UseBlankTriggerAddingConvention(
            this ModelConfigurationBuilder configurationBuilder)
        {
            ArgumentNullException.ThrowIfNull(configurationBuilder);

            configurationBuilder.Conventions.Add(_ => new Conventions.BlankTriggerAddingConvention());
            return configurationBuilder;
        }
    }
}
