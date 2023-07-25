namespace LibraryMVC.Models
{
    public class Library
    {
        public Library()
        {
            title = "";
            books = new BookContainer();
            authors = new AuthorContainer();
        }
        public string title;
        public BookContainer books = new BookContainer();
        public AuthorContainer authors = new AuthorContainer();
        public List<User> users;

        public void setTitle(string title) {  this.title = title; }
        public BookContainer GetBooks() { return books; }
        public AuthorContainer GetAuthors() { return authors;  }
        public void AddBook(string title, string authorName, int authorBD, string category, int publishDate, int quantity)
        {
            foreach (Book b in books.Books)
            {
                if (b.title == title && b.author.name == authorName)
                {
                    b.quantity++;
                    return;
                }
            }
            Book newBook = new Book(title, authorName, authorBD, category, publishDate, quantity);
            AddAuthor(authorName, authorBD, "");
            books.Add(newBook);
        }

        public void AddBook(Book book)
        {
            foreach (Book b in books.Books)
            {
                if (b.title == book.title && b.author.name == book.author.name)
                {
                    b.quantity++;
                    return;
                }
            }
            AddAuthor(book.author.name, book.author.birthdate, "");
            books.Add(book);
        }

        public void AddAuthor(string name, int BD, string bio)
        {
            foreach (Author a in authors.Authors)
            {
                if (a.name == name && a.birthdate == BD)
                {
                    a.bio = bio;
                    return;
                }
                else if (a.bio == bio && a.name == name)
                {
                    a.birthdate = BD;
                    return;
                }
                else if (a.birthdate == BD && a.bio == bio)
                {
                    a.name = name;
                    return;
                }
            }
            Author newAuthor = new Author(name, BD, bio);
            authors.Add(newAuthor);
        }

    }
}
