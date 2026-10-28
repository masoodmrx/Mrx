namespace Mrx.AspNetCore.Services
{
    [Service(ServiceLifetime.Singleton)]
    public class GlobalVariableService
    {
        public object this[string key]
        {
            get => data.GetValueOrDefault(key);
            set => data[key] = value;
        }
        private readonly Dictionary<string, object> data = new Dictionary<string, object>();
    }
}
