using Domain;

namespace Persistence;

public static class Seed
{
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private static readonly Func<int, string> Symbols = n => string
        .Join("", Enumerable
            .Range(0, n)
            .Select(_ => Alphabet[Random.Shared.Next(Alphabet.Length)]));

    private static int Number => Random.Shared.Next(0, 1000);

    private static Book CreateBook()
    {
        var book = new Book()
        {
            Author = $"{Symbols(4)} Author",
            Content = $"Content {Number}",
            Cover = $"Cover {Number}",
            Genre = Constants.Genres[Random.Shared.Next(Constants.Genres.Count)],
            Title = $"{Symbols(4)} Title"
        };
        var reviews = Enumerable.Range(0, Random.Shared.Next(20))
            .Select(_ => new Review
            {
                Book = book,
                Message = $"Message {Number}",
                Reviewer = $"{Symbols(4)} Reviewer"
            }).ToList();
        var ratings = Enumerable.Range(0, Random.Shared.Next(20))
            .Select(_ => new Rating
            {
                Score = Random.Shared.Next(1, 6)
            }).ToList();
        book.Reviews = reviews;
        book.Ratings = ratings;
        return book;
    }

    public static void SeedData(DataContext dataContext)
    {
        var books = Enumerable.Range(0, 20).Select(_ => CreateBook()).ToList();
        dataContext.AddRange(books);
        dataContext.SaveChanges();
    }
}