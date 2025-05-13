using Microsoft.AspNetCore.Diagnostics;
using TodoApp.Common.Exceptions;
using TodoApp.Common.ResponseHanlder;

namespace TodoApp.Api.ExceptionHandler;

public static class ExceptionMiddlewareExtensions
{
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                var ex = exceptionHandlerPathFeature?.Error;

                context.Response.ContentType = "application/json";

                var statusCode = ex switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                    NotValidException => StatusCodes.Status400BadRequest,
                    ConflictException => StatusCodes.Status409Conflict,
                    _ => StatusCodes.Status500InternalServerError
                };

                context.Response.StatusCode = statusCode;

                await context.Response.WriteAsJsonAsync(new ApiResponse
                {
                    Message = ex?.Message ?? string.Empty,
                    CodeStatus = statusCode
                });
            });
        });
    }
}