namespace LibraryMVC.Models
{
    public class BookContainer
    {
        public BookContainer()
        {
            Books = new List<Book>();
        }
        public List<Book> Books = new List<Book>();
        public void Add(Book b) { Books.Add(b); }
        public void Remove(Book b) { Books.Remove(b); }
        public Book Get(string title, string author)
        {
            if (Books.Count != 0)
            {
                foreach (Book b in Books)
                {
                    if (b.title == title && b.author.name == author)
                    {
                        return b;
                    }
                }
                return null;
            }
            return null;
        }
    }
}
