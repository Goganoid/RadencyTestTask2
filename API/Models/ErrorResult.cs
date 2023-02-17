namespace API.Models;

public class ErrorResult
{
    public List<string> Messages { get; set; } = new();

    public string? Source;
    public string? Exception;
    public string? ErrorId;
    public int StatusCode;
}