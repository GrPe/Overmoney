using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Overmoney.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Overmoney.Api.Infrastructure;

public sealed class ExceptionHandler : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch(ValidationException ex)
        {
            await HandleProblemDetails(context, string.Join(Environment.NewLine, ex.Errors));
        }
        catch(DomainValidationException ex)
        {
            await HandleProblemDetails(context, ex.Message);
        }
    }

    private static async Task HandleProblemDetails(HttpContext context, string errorDetails)
    {
        var details = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "Validation error",
            Status = (int)HttpStatusCode.BadRequest,
            Instance = context.Request.Path,
            Detail = errorDetails
        };

        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(details));
    }
}
