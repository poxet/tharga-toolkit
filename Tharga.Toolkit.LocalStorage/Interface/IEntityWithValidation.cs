namespace Tharga.Toolkit.LocalStorage.Interface
{
    public interface IEntityWithValidation : IEntity
    {
        bool IsValid { get; }
        string Error { get; }
    }
}