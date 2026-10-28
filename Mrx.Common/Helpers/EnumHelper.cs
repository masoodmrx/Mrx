namespace Mrx.Common.Helpers
{
    public static class EnumHelper
    {
        public static IEnumerable<TEnum> GetValues<TEnum>() where TEnum : Enum
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }
    }
}
