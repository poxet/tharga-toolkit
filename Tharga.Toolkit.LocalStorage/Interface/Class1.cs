using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleInjector;
using SimpleInjector.Extensions;
using Tharga.Toolkit.LocalStorage.Business;

namespace Tharga.Toolkit.LocalStorage.Interface
{
    public interface IDeleteHandler<TDto>
    {
        void Handle(TDto dto, DateTime? previousServerStoreTime);
    }

    public interface ISavedHandler<TDto>
    {
        void Handle(TDto dto, DateTime? previousServerStoreTime);
    }

    public interface ISyncHandler<TDto>
    {
        void Handle(SubscriptionCallbackBase sc, object[] changed, object[] deleted);
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
            var assemblies = new List<Assembly> {Assembly ?? Assembly.GetEntryAssembly()};
            //var entry = Assembly.GetEntryAssembly();
            //var asms = entry.GetReferencedAssemblies();
            ////var assemblies = asms.Select(x => Assembly.GetAssembly() ).ToList();


            container.RegisterManyForOpenGeneric(
                typeof(ISavedHandler<>), assemblies);

            container.RegisterManyForOpenGeneric(
                typeof(ISyncHandler<>), assemblies);

            container.RegisterManyForOpenGeneric(
                typeof(IDeleteHandler<>), assemblies);

            //container.RegisterManyForOpenGeneric(
            //    typeof(IQueryHandler<,>), assemblies);

            container.Verify();

            return container;
        }

        public static object GetInstance(Type serviceType)
        {
            //if (_container == null)
            //    throw new NotImplementedException("The Bootstrapper has not yet been initiated.");
            //return _container.GetInstance(serviceType);
            return Instance.GetInstance(serviceType);
        }
    }
}
