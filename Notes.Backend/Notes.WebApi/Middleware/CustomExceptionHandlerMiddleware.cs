using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using FluentValidation.Results;
using System.Text.Json.Serialization;
using Notes.Application.Common.Exceptions;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Notes.WebApi.Middleware;

public class CustomExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    public CustomExceptionHandlerMiddleware(RequestDelegate _next)
    {
        this._next = _next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = HttpStatusCode.InternalServerError;
        var result = string.Empty;

        switch (exception)
        {
            case FluentValidation.ValidationException validationException:
                statusCode = HttpStatusCode.BadRequest;
                result = JsonSerializer.Serialize(validationException.Errors);
                break;
            case NotFoundException notFoundException: 
                statusCode = HttpStatusCode.NotFound;
                break;
        }
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        if ( result == string.Empty )
        {
            result = JsonSerializer.Serialize(new { error = exception.Message });
        }

        return context.Response.WriteAsync(result);

    }
}
