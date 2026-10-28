namespace Mrx.AspNetCore.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class CheckModelStateAttribute : Attribute
    {
    }
}
