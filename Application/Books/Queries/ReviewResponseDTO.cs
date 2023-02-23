namespace Application.Books.Queries;

public class ReviewResponseDTO
{
    public int ReviewId { get; set; }
    public required string Message { get; set; }
    public required string Reviewer { get; set; }
}