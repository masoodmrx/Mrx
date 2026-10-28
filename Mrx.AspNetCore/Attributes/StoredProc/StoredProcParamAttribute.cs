namespace Mrx.AspNetCore.Attributes.StoredProc
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StoredProcParamAttribute : Attribute
    {
        public StoredProcParamAttribute(int size = 0, byte precision = 0, byte scale = 0)
        {
            Size = size;
            Precision = precision;
            Scale = scale;
        }

        public int Size { get; }
        public byte Precision { get; }
        public byte Scale { get; }
    }
}
