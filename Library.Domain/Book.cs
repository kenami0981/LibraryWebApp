namespace Library.Domain
{
    public enum BookGenre
    {
        Fiction,
        NonFiction,
        ScienceFiction,
        Biography,
        Mystery,
        Romance,
        Fantasy,
        History,
        Thriller,
        SelfHelp
    }
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public BookGenre Genre { get; set; }
        public Guid AuthorId { get; set; } //klucz obcy
        public Author Author { get; set; } = null!;
        public DateTime PublishedDate { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public int PageCount { get; set; }
        public string Publisher { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }

    }
}
