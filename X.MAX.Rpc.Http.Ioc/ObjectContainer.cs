using Autofac;
using System;

namespace X.MAX.Rpc.Http.Ioc
{
    public class ObjectContainer : IObjectContainer
    {
        ContainerBuilder _Builder;
        IContainer _Container;

        public ObjectContainer()
        {
            _Builder = new ContainerBuilder();
            //autoregister

            _Container = _Builder.Build();
        }

        public object Resolve(string fullName)
        {
            using (var scope = _Container.BeginLifetimeScope())
            {
                var obj = scope.Resolve(Type.GetType(fullName));
                return obj;
            }
        }
    }
}
