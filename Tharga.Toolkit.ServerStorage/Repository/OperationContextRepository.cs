using System.ServiceModel;
using Tharga.Toolkit.ServerStorage.Interface;

namespace Tharga.Toolkit.ServerStorage.Repository
{
    public class OperationContextRepository<T> : IOperationContextRepository<T>
    {
        public T GetCallbackChannel()
        {
            return OperationContext.Current.GetCallbackChannel<T>();
        }
    }
}