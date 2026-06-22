namespace EcoSystem.Business.Common;

public sealed record ServiceResult<T>(
    bool IsSuccess,
    int StatusCode,
    string Message,
    T? Data = default,
    IReadOnlyCollection<string>? Errors = null)
{
    public static ServiceResult<T> Success(T? data, string message = "Operación completada.", int statusCode = 200)
        => new(true, statusCode, message, data);

    public static ServiceResult<T> Created(T data, string message = "Recurso creado correctamente.")
        => new(true, 201, message, data);

    public static ServiceResult<T> NoContent(string message = "Recurso eliminado correctamente.")
        => new(true, 204, message, default);

    public static ServiceResult<T> Failure(int statusCode, string message, params string[] errors)
        => new(false, statusCode, message, default, errors);
}
