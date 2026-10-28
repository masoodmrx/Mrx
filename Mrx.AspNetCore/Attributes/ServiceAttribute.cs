using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Mrx.AspNetCore
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
