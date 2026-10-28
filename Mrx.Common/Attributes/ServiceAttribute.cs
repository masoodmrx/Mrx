namespace Mrx.Common.Attributes
{
    public class ServiceAttribute : Attribute
    {
        public ServiceLifetime ServiceLifetime { get; }
        public Type ServiceType { get; }

        public ServiceAttribute(ServiceLifetime serviceLifetime = ServiceLifetime.Scoped, Type serviceType = null)
        {
            ServiceLifetime = serviceLifetime;
            ServiceType = serviceType;
        }
        public ServiceAttribute(Type serviceType)
        {
            ServiceLifetime = ServiceLifetime.Scoped;
            ServiceType = serviceType;
        }
    }
}
