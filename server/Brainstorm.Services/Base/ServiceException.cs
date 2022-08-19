namespace Brainstorm.Services;
public class ServiceException<T> : Exception
{
    static string BuildMessage(string method) =>
        $"{typeof(T)} {method} method error";

    public ServiceException(string method, Exception ex)
        : base(BuildMessage(method), ex) { }
}