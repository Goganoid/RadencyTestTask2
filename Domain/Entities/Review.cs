namespace Domain.Entities;

public class Review
{
    public int ReviewId { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }
    public string Message { get; set; }
    public string Reviewer { get; set; }
}