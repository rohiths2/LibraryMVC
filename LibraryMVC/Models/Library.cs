using System.Reflection;

namespace LibraryMVC.Models
{
    public class Library
    {
        public Library()
        {
            title = "";
            books = new BookContainer();
            authors = new AuthorContainer();
            users.Add(new User("Admin1", "admin"));
        }
        public User currentUser = new User("Admin1", "admin");
        public List<Order> orders = new List<Order>();
        public string title;
        public string recentOrder = ".";
        public BookContainer books = new BookContainer();
        public AuthorContainer authors = new AuthorContainer();
        public List<User> users = new List<User>();
        public List<Book> bookSearchResults = new List<Book>();
        public List<string[]> categoryInfo = new List<string[]>();

        public void addCategory(string name, string description)
        {
            if (currentUser.role != "admin" && currentUser.role != "librarian")
            {
                return;
            }
                foreach (string[] category in categoryInfo)
            {
                if (category[0] == name)
                {
                    category[1] = description;
                    return;
                }
                if (category[1] == description)
                {
                    category[0] = name;
                    return;
                }
            }
            categoryInfo.Add(new string[] { name, description });
        }

        public List<string> recentlyBorrowedBooks = new List<string>();

        public string borrowBook(string title, string authorName)
        {
            foreach (Book b in books.Books)
            {
                if (b.title == title && b.author.name ==  authorName && b.quantity > 0)
                {
                    recentlyBorrowedBooks.Insert(0, b.title);
                    b.quantity = b.quantity - 1;
                    orders.Add(new Order(currentUser.username, b.title, DateTime.Today.AddDays(7)));
                    return "You borrowed " + b.title + ". The due date is " + DateTime.Today.AddDays(7).ToString();
                }
            }
            return "This book is unavailable at this time. Check the List of Books and try again.";
        }

        public void returnBook(string title, string authorName)
        {
            foreach (Book b in books.Books)
            {
                if (b.title == title && b.author.name == authorName)
                {
                    b.quantity = b.quantity + 1;
                }
            }
            foreach (Order o in orders)
            {
                if (o.bookTitle == title)
                {
                    o.returned = "Returned";
                    break;
                }
            }
        }

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
