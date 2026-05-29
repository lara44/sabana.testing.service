namespace Presentation.Common;

public class ResponseDto<T>
{
    public int Code { get; set; } = 200;
    public bool Error { get; set; }
    public string? Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    private ResponseDto(int code = 200, bool error = false, string? message = null, T? data = default)
    {
        Code = code;
        Error = error;
        Message = message;
        Data = data;
    }

    public ResponseDto()
    {
    }

    public static ResponseDto<T> Success(string? message = null, T? data = default)
    {
        return new ResponseDto<T>(200, false, message, data);
    }

    public static ResponseDto<T> ErrorGeneric(int code = 400, string? message = null, T? data = default)
    {
        return new ResponseDto<T>(code, true, message, data);
    }

    public static ResponseDto<T> ErrorBadRequest(string? message = null, T? data = default)
    {
        return new ResponseDto<T>(400, true, message, data);
    }

    public static ResponseDto<T> ErrorNotFound(string? message = null, T? data = default)
    {
        return new ResponseDto<T>(404, true, message, data);
    }

    public static ResponseDto<T> ErrorUnauthorized(string? message = null, T? data = default)
    {
        return new ResponseDto<T>(401, true, message, data);
    }

    public static ResponseDto<T> ErrorForbidden(string? message = null, T? data = default)
    {
        return new ResponseDto<T>(403, true, message, data);
    }

    public static ResponseDto<T> ErrorInternalServerError(string? message = null, T? data = default)
    {
        return new ResponseDto<T>(500, true, message, data);
    }
}