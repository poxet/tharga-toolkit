using Tharga.Toolkit.ServerStorage.Interface;
using Tharga.Toolkit.ServerStorage.Repository;

namespace Tharga.Toolkit.ServerStorage
{
    public class OperationContectContainer<T>
    {
        private static IOperationContextRepository<T> _operationContextRepositoryInstance;

        public static IOperationContextRepository<T> OperationContextRepository
        {
            get { return _operationContextRepositoryInstance ?? (_operationContextRepositoryInstance = new OperationContextRepository<T>()); }
            set { _operationContextRepositoryInstance = value; }
        }
    }
}