using Microsoft.AspNetCore.Http;

namespace TodoApp.Common.ResponseHanlder;

public class ApiResponse<T>
{
    public string? Message { get; set; }
    public T? Data { get; set; }
    public int CodeStatus { get; set; }

    public static ApiResponse<T> SuccessResponse(T data, string? message = null, int codeStatus = 200)
        => new() { Data = data, Message = message, CodeStatus = codeStatus };

    public static ApiResponse<T> FailureResponse(string message, int codeStatus = 500)
        => new() { Data = default, Message = message, CodeStatus = codeStatus };
}

public class ApiResponse
{
    public string? Message { get; set; }
    public int CodeStatus { get; set; }

    public static ApiResponse SuccessResponse(string? message = null, int codeStatus = 200)
        => new() { Message = message, CodeStatus = codeStatus };

    public static ApiResponse FailureResponse(string message, int codeStatus = 500)
        => new() { Message = message, CodeStatus = codeStatus };
}