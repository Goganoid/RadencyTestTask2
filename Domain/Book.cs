using System.Linq.Expressions;

namespace Domain;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Cover { get; set; }
    public string Content { get; set; }
    public string Author { get; set; }
    public string Genre { get; set; }

    public List<Rating> Ratings { get; set; } = new();
    public List<Review> Reviews { get; set; } = new();
}

public static class BookExtensions
{
    public static double AverageScore(this Book book)
    {
        return book.Ratings.Count > 0 ? Math.Round(book.Ratings.Average(b => b.Score), 2) : 0;
    }
}