namespace Application.Books.DTO;

public class ListedBookResponseDTO
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public double Rating { get; set; }
    public int Reviews { get; set; }
}