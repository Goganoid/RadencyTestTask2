using System.Net;

namespace Application.Core.Exceptions;

public class BadRequestException : CustomException
{
    public BadRequestException(string message) : base(message, null, HttpStatusCode.BadRequest)
    {
    }
}