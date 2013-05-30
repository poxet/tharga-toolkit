namespace Tharga.Toolkit.ServerStorage.Interface
{
    public interface IOperationContextRepository<out T>
    {
        T GetCallbackChannel();
    }
}