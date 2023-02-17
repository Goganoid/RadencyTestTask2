using System.Net;
using System.Text.Json;
using API.Models;
using Application.Core.Exceptions;

namespace API.Middlewares;

public class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            var errorId = Guid.NewGuid().ToString();
            var errorResult = new ErrorResult
            {
                Source = exception.TargetSite?.DeclaringType?.FullName,
                Exception = exception.Message.Trim(),
                ErrorId = errorId
            };
            errorResult.Messages.Add(exception.Message);
            // get the deepest exception
            if (exception is not CustomException && exception.InnerException != null)
                while (exception.InnerException != null)
                    exception = exception.InnerException;

            switch (exception)
            {
                case CustomException e:
                    errorResult.StatusCode = (int) e.StatusCode;
                    if (e.ErrorMessages is not null) errorResult.Messages = e.ErrorMessages;

                    break;
                case KeyNotFoundException:
                    errorResult.StatusCode = (int) HttpStatusCode.NotFound;
                    break;
                default:
                    errorResult.StatusCode = (int) HttpStatusCode.InternalServerError;
                    break;
            }

            _logger.LogError(exception, "Request failed with Status Code {ResponseStatusCode} and Error Id {ErrorId}",
                errorResult.StatusCode, errorId);
            var response = context.Response;
            if (!response.HasStarted)
            {
                response.ContentType = "application/json";
                response.StatusCode = errorResult.StatusCode;
                await response.WriteAsync(JsonSerializer.Serialize(errorResult));
            }
            else
            {
                _logger.LogWarning("Can't write error response. Response has already started");
            }
        }
    }
}