namespace Application.Books.Queries;

public class ListedBookResponseDTO
{
    public int BookId { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public required string Cover { get; set; }
    public double Rating { get; set; }
    public int Reviews { get; set; }
}