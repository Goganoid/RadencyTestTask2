using System.Net;

namespace Application.Books.Commands.Core.Exceptions;

public class BadRequestException : CustomException
{
    public BadRequestException(string message) : base(message, null, HttpStatusCode.BadRequest)
    {
    }
}