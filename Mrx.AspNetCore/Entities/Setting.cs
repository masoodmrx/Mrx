namespace Mrx.AspNetCore.Entities
{
    public class Setting
    {
        [Key]
        public string Key { get; set; }
        public string? Value { get; set; }
        public string? Collection { get; set; }
    }
}
