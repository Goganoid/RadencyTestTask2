using System.Net;
using System.Text.Json;
using Application.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(exceptionHandlerApp =>
        {
            exceptionHandlerApp.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                var exceptionHandlerPathFeature =
                    context.Features.Get<IExceptionHandlerPathFeature>();
                var error = exceptionHandlerPathFeature?.Error;
                context.Response.StatusCode = error switch
                {
                    BadRequestException => (int) HttpStatusCode.BadRequest,
                    ConflictException => (int) HttpStatusCode.Conflict,
                    NotFoundException => (int) HttpStatusCode.NotFound,
                    _ => (int) HttpStatusCode.InternalServerError
                };
                var result = JsonSerializer.Serialize(new { message = error?.Message });
                await context.Response.WriteAsync(result);
            });
        });
    }
}