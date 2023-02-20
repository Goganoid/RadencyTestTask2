namespace Application.Books.DTO.Responses;

public class ListedBookResponseDTO
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Cover { get; set; }
    public double Rating { get; set; }
    public int Reviews { get; set; }
}