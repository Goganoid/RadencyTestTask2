namespace Application.Books.DTO;

public class ReviewResponseDTO
{
    public int Id { get; set; }
    public required string Message { get; set; }
    public required string Reviewer { get; set; }
}