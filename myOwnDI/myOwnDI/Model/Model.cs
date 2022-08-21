using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myOwnDI.Model
{
    public enum LifeTime
    {
        Trancient,
        Scoped,
        Singleton
    }

    public interface IContainerBuilder
    {
        void Register(ServiceDescriptor descriptor);

        IContainer Build();
    }

    public class ContainerBuilder : IContainerBuilder
    {
        private readonly List<ServiceDescriptor> _descriptors = new();


        public IContainer Build()
        {
            return new Container(_descriptors);
        }

        public void Register(ServiceDescriptor descriptor)
        {
            _descriptors.Add(descriptor);
        }
    }

    public class Container : IContainer
    {
        private class Scope : IScope
        {
            private readonly Container _container;
            public Scope(Container container)
            {
                _container = container;
            }
            public object Resolve(Type service) => _container.CreateInstance(service, this);
        }

        private readonly Dictionary<Type, ServiceDescriptor> _descriptors;

        public Container(IEnumerable<ServiceDescriptor> descriptors)
        {
            _descriptors = descriptors.ToDictionary(x => x.ServiceType);
        }

        public IScope CreateScope()
        {
            return new Scope(this);
        }

        private object CreateInstance(Type service, IScope scope)
        {
            if (!_descriptors.TryGetValue(service, out var descriptor))
                throw new InvalidOperationException($"Service {service} is not registered.");

            if (descriptor is InstanceBasedServiceDescriptor ib)
                return ib.Instance;
            if (descriptor is FactoryBasedServiceDescriptor fb)
                return fb.Factory(scope);

            var tb = (TypeBasedServiceDescriptor)descriptor;
            var ctor = tb.ImplementionType.GetConstructors(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Single();
            var args = ctor.GetParameters();
            var argsForCtor = new object[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                System.Reflection.ParameterInfo? arg = args[i];
                argsForCtor[i] = CreateInstance(args[i].ParameterType, scope);
            }
            return ctor.Invoke(argsForCtor);
        }
    }

    public interface IContainer
    {
        IScope CreateScope();
    }

    public interface IScope
    {
        object Resolve(Type service);
    }
}
