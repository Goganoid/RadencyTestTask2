using Domain;
using Persistence;

namespace Application.Books.DTO;

public class BookDetailsResponseDTO
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Cover { get; set; }
    public required string Content { get; set; }
    public required string Author { get; set; }
    public required string Genre { get; set; }
    public double Rating { get; set; }
    public List<ReviewResponseDTO> Reviews { get; set; } = new();
}