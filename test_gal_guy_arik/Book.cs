namespace test_gal_guy_arik
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Serial { get; set; }
        public Genre Genre { get; set; }
        public bool IsAvailable { get; set; }

        public Book(string title, string author, string isbn, Genre genre)
        {
            Title = title;
            Author = author;
            Serial = isbn;
            Genre = genre;
            IsAvailable = true;
        }

        public virtual string DisplayInfo()
        {
            return $"{Title} by {Author}, Serial: {Serial}, Genre: {Genre}, Available: {IsAvailable}";
        }
    }
}