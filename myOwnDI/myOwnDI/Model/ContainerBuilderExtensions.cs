using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myOwnDI.Model
{

    public static class ContainerBuilderExtensions 
    {
        private static IContainerBuilder RegisterType(this IContainerBuilder builder, Type service, Type implementation, LifeTime lifeTime)
        {
            builder.Register(new TypeBasedServiceDescriptor()
            {
                ImplementionType = implementation,
                ServiceType = service,
                LifeTime = lifeTime
            });
            return builder;
        }

        private static IContainerBuilder RegisterInstance(this IContainerBuilder builder, Type service, object instance)
        {
            builder.Register(new InstanceBasedServiceDescriptor(service, instance));
            return builder;
        }

        private static IContainerBuilder RegisterFactory(this IContainerBuilder builder, Type service, Func<IScope, object> factory, LifeTime lifeTime)
        {
            builder.Register(new FactoryBasedServiceDescriptor()
            {
                Factory = factory,
                ServiceType = service,
                LifeTime = lifeTime
            });
            return builder;
        }

        public static IContainerBuilder RegisterSingleton(this IContainerBuilder builder, Type service, Type implementation)
            => builder.RegisterType(service, implementation, LifeTime.Singleton);
        public static IContainerBuilder RegisterTrancient(this IContainerBuilder builder, Type service, Type implementation)
            => builder.RegisterType(service, implementation, LifeTime.Trancient);
        public static IContainerBuilder RegisterScoped(this IContainerBuilder builder, Type service, Type implementation)
            => builder.RegisterType(service, implementation, LifeTime.Scoped);
        public static IContainerBuilder RegisterTrancient<TService, TImplementation>(this IContainerBuilder builder) where TImplementation : TService
            => builder.RegisterType(typeof(TService), typeof(TImplementation), LifeTime.Trancient);
        public static IContainerBuilder RegisterScoped<TService, TImplementation>(this IContainerBuilder builder) where TImplementation : TService
            => builder.RegisterType(typeof(TService), typeof(TImplementation), LifeTime.Scoped);
        public static IContainerBuilder RegisterSingleton<TService, TImplementation>(this IContainerBuilder builder) where TImplementation : TService
            => builder.RegisterType(typeof(TService), typeof(TImplementation), LifeTime.Singleton);

        public static IContainerBuilder RegisterTrancient<TService>(this IContainerBuilder builder, Func<IScope, TService> factory)
            => builder.RegisterFactory(typeof(TService), s => factory(s), LifeTime.Trancient);
        public static IContainerBuilder RegisterSingleton<TService>(this IContainerBuilder builder, Func<IScope, TService> factory)
            => builder.RegisterFactory(typeof(TService), s => factory(s), LifeTime.Singleton);
        public static IContainerBuilder RegisterScoped<TService>(this IContainerBuilder builder, Func<IScope, TService> factory)
            => builder.RegisterFactory(typeof(TService), s => factory(s), LifeTime.Scoped);

        public static IContainerBuilder RegisterSingleton<T>(this IContainerBuilder builder, object instance)
            => builder.RegisterInstance(typeof(T), instance);
    }

}
