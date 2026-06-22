using EcoSystem.Business.Common;

namespace EcoSystem.API.Controllers;

public sealed record ApiResponse<T>(string Message, T? Data = default, IReadOnlyCollection<string>? Errors = null)
{
    public static ApiResponse<T> From(ServiceResult<T> result)
        => new(result.Message, result.Data, result.Errors);
}
