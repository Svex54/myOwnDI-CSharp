using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myOwnDI.Model
{
    public abstract class ServiceDescriptor
    {
        public Type ServiceType { get; init; }
        public Type ImplementionType { get; init; }
        public LifeTime LifeTime { get; init; }
    }

    public class FactoryBasedServiceDescriptor : ServiceDescriptor
    {
        public Func<IScope, object> Factory { get; init; }
    }

    public class TypeBasedServiceDescriptor : ServiceDescriptor
    {
        public Type ImplementionType { get; init; }
    }

    public class InstanceBasedServiceDescriptor : ServiceDescriptor
    {
        public object Instance { get; init; }

        public InstanceBasedServiceDescriptor(Type serviceType, object instance)
        {
            LifeTime = LifeTime.Singleton;
            ServiceType = serviceType;
            Instance = instance;
        }
    }
}
