using System.Collections.Generic;
using System.Reflection;

namespace Mrx.AspNetCore
{
    public class AutoMapperOptions
    {
        public List<Assembly> Assemblies { get; } = new List<Assembly>();
    }
}
