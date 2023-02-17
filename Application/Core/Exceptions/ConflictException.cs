using System.Net;

namespace Application.Core.Exceptions;

public class ConflictException : CustomException
{
    public ConflictException(string message) : base(message, null, HttpStatusCode.Conflict)
    {
    }
}