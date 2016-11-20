using System;
using System.Collections.Generic;
using System.Reflection;
using SimpleInjector;
using SimpleInjector.Extensions;
using Tharga.Toolkit.LocalStorage.Interface;

namespace Tharga.Toolkit.LocalStorage.Utility
{
    public static class Bootstrapper
    {
        private static readonly Lazy<Container> InstanceLoader = new Lazy<Container>(GetContainer);
        private static Container Instance { get { return InstanceLoader.Value; } }

        public static Assembly Assembly { private get; set; }

        private static Container GetContainer()
        {
            var container = new Container();

            var assemblies = new List<Assembly> { Assembly ?? Assembly.GetExecutingAssembly() };

            container.Register(typeof(ISavedHandler<>), assemblies);
            container.Register(typeof(ISyncHandler<>), assemblies);
            container.Register(typeof(IDeleteHandler<>), assemblies);

            container.Verify();

            return container;
        }

        public static object GetInstance(Type serviceType)
        {
            return Instance.GetInstance(serviceType);
        }
    }
}
