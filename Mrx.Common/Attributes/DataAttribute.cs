namespace Mrx.Common.Attributes
{
    public class DataAttribute : Attribute
    {
        public DataAttribute(object value = null)
        {
            Value = value;
        }
        public object Value { get; }
    }
}
