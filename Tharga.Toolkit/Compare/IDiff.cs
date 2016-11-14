namespace Tharga.Toolkit
{
    public interface IDiff
    {
        string Message { get; }
        string ObjectName { get; }
        string OtherObjectName { get; }
        int? Index { get; }
    }
}