using System;
using System.Collections.Generic;
using System.Reflection;
using SimpleInjector;
using SimpleInjector.Extensions;
using Tharga.Toolkit.ServerStorage.Interface;

namespace Tharga.Toolkit.ServerStorage.Utility
{
    public static class Bootstrapper
    {
        private static readonly Lazy<Container> InstanceLoader = new Lazy<Container>(GetContainer);
        private static Container Instance { get { return InstanceLoader.Value; } }

        public static Assembly Assembly { private get;  set; }

        private static Container GetContainer()
        {
            var container = new Container();

            var assemblies = new List<Assembly> { Assembly, Assembly.GetExecutingAssembly() };

            container.Register(typeof(ICommandHandler<>), assemblies);
            container.Register(typeof(IMessageHandler<>), assemblies);
            container.Register(typeof(ICreateSessionHandler<>), assemblies);
            container.Register(typeof(IEndSessionHandler<>), assemblies);

            container.Verify();

            return container;
        }

        public static object GetInstance(Type serviceType)
        {
            return Instance.GetInstance(serviceType);
        }
    }
}