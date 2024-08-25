using EnhancedLibrarySystem;
using test_gal_guy_arik;

public class TextBook : Book
{
    public string Subject { get; set; }

    public TextBook(string title, string author, string isbn, Genre genre, string subject)
        : base(title, author, isbn, genre)
    {
        Subject = subject;
    }

    public override string DisplayInfo()
    {
        return $"{base.DisplayInfo()}, Subject: {Subject}";
    }
}