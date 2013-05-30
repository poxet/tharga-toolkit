using System;
using System.Collections.Generic;
using System.Reflection;
using SimpleInjector;
using SimpleInjector.Extensions;

namespace Tharga.Toolkit.ServerStorage.Interface
{
    public interface ICommandHandler<TCommand>
    {
        void Handle(TCommand customerInputDto); //, IServiceMessageBase serviceMessage
    }

    public interface IMessageHandler<TCommand>
    {
        void Handle(TCommand customerInputDto); //, IServiceMessageBase serviceMessage);
    }

    public static class Bootstrapper
    {
        private static readonly Lazy<Container> _instanceLoader = new Lazy<Container>(GetContainer);
        private static Container Instance { get { return _instanceLoader.Value; } }

        public static Assembly Assembly { get; set; }

        private static Container GetContainer()
        {
            var container = new Container();

            //var assemblies = BuildManager
            //    .GetReferencedAssemblies().Cast<Assembly>();
            var assemblies = new List<Assembly> { Assembly ?? Assembly.GetEntryAssembly() };

            container.RegisterManyForOpenGeneric(
                typeof(ICommandHandler<>), assemblies);

            container.RegisterManyForOpenGeneric(
                typeof(IMessageHandler<>), assemblies);

            //container.RegisterManyForOpenGeneric(
            //    typeof(IQueryHandler<,>), assemblies);

            container.Verify();

            return container;
        }

        public static object GetInstance(Type serviceType)
        {
            return Instance.GetInstance(serviceType);
        }
    }
}
