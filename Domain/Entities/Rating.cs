namespace Domain.Entities;

public class Rating
{
    public int RatingId { get; set; }
    public int BookId { get; set; }
    public Book Book { get; set; }
    public int Score { get; set; }
}