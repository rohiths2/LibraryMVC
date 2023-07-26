using LibraryMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Xml.Linq;

namespace LibraryMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Book book1 = new Book("title1", "author1", 010101, "A", 010123, 3);
        Book book2 = new Book("title2", "author2", 010101, "A", 010123, 2);
        Book book3 = new Book("title3", "author1", 010101, "B", 010123, 1);
        public static Library library = new Library();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public bool BookListContains(Book b)
        {
            if (library.books.Books.Count == 0)
            {
                return false;
            }
            foreach (Book s in library.books.Books)
            {
                if (s.title == b.title && s.author.name == b.author.name)
                {
                    return true;
                }
            }
            return false;
        }

        public bool AuthorListContains(Author a)
        {
            if (library.authors.Authors.Count == 0)
            {
                return false;
            }
            foreach (Author s in library.authors.Authors)
            {
                if (s.name == a.name)
                {
                    return true;
                }
            }
            return false;
        }

        public bool userExists(string username_, string password_)
        {
            foreach (User u in library.users)
            {
                if (u.username == username_ && u.password == password_) {
                    return true;
                }
            }
            return false;
        }



        public IActionResult Index()
        {
            string[] credentials = new string[6];
            library.setTitle("MVC Library");
            return View(credentials);
        }  

        [HttpPost]
        public IActionResult Index(string[] credentials)
        {
            if (userExists(credentials[0], credentials[1])) {
                library.currentUser.username = credentials[0];
                library.currentUser.password = credentials[1];
                foreach (User u in library.users)
                {
                    if (u.username == credentials[0])
                    {
                        library.currentUser.role = u.role;
                        break;
                    }
                }
                return RedirectToAction("BookList");
            } else
            {
                return RedirectToAction("Index");
            }
        }

        public IActionResult CreateAccount()
        {
            string[] credentials = new string[6];
            return View(credentials);
        }


        [HttpPost]
        public IActionResult CreateAccount(string[] credentials)
        {
            User created = new User(credentials[0], credentials[2]);
            created.password = credentials[1];
            library.users.Add(created);
            return RedirectToAction("Index");
        }

        public IActionResult BookList()
        {
            if (!BookListContains(book1)) { library.books.Books.Add(book1); }
            if (!BookListContains(book2)) { library.books.Books.Add(book2); }
            if (!BookListContains(book3)) { library.books.Books.Add(book3); }
            if (!AuthorListContains(book1.author)) { library.authors.Authors.Add(book1.author); }
            if (!AuthorListContains(book2.author)) { library.authors.Authors.Add(book2.author); }
            library.authors.Authors.ElementAt(0).bio = "bio for author 1";
            library.authors.Authors.ElementAt(1).bio = "bio for author 2";
            return View(library);
        }

        public IActionResult CheckOut()
        {
            string[] info = new string[3];
            return View(info);
        }

        [HttpPost]
        public IActionResult CheckOut(string[] info)
        {
            if (info[0] == "borrow")
            {
                library.recentOrder = library.borrowBook(info[1], info[2]);
            } else if (info[0] == "return")
            {
                library.returnBook(info[1], info[2]);
                library.recentOrder = "Returned";
            } else
            {
                library.recentOrder = "Please try again: make sure you typed borrow or return correctly.";
            }
            return RedirectToAction("BookList");
        }

        public IActionResult CreateNewBook()
        {
            if (library.currentUser.role != "admin" && library.currentUser.role != "librarian")
            {
                library.recentOrder = "Edits cannot be performed by members (only admins or librarians)";
                return RedirectToAction("BookList");
            }
            string[] newBookInfo = new string[6];
            return View(newBookInfo);
        }

        [HttpPost]
        public IActionResult CreateNewBook(string[] newBookInfo)
        {
            library.AddBook(new Book(newBookInfo[0], newBookInfo[1], Convert.ToInt32(newBookInfo[2]), newBookInfo[3], Convert.ToInt32(newBookInfo[4]), Convert.ToInt32(newBookInfo[5])));
            return RedirectToAction("BookList");
        }
        public IActionResult AuthorList()
        {
            return View(library);
        }

        public IActionResult CreateNewAuthor()
        {
            if (library.currentUser.role != "admin" && library.currentUser.role != "librarian")
            {
                library.recentOrder = "Edits cannot be performed by members (only admins or librarians)";
                return RedirectToAction("BookList");
            }
            string[] newAuthorInfo = new string[3];
            return View(newAuthorInfo);
        }

        [HttpPost]
        public IActionResult CreateNewAuthor(string[] newAuthorInfo)
        {
            library.AddAuthor(newAuthorInfo[0], Convert.ToInt32(newAuthorInfo[1]), newAuthorInfo[2]);
            return RedirectToAction("AuthorList");
        }

        public IActionResult Report()
        {
            return View(library);
        }

        public IActionResult SearchOptions()
        {
            string[] filters = new string[2];
            return View(filters);
        }

        [HttpPost]
        public IActionResult SearchOptions(string[] filters)
        {
            if (filters[0] == "title")
            {
                foreach (Book b in library.books.Books)
                {
                    string[] split = b.title.Split(' ');
                    foreach (string s in split)
                    {
                        if (s == filters[1] && !library.bookSearchResults.Contains(b))
                        {
                            library.bookSearchResults.Add(b);
                        }
                    }
                }
            } else if (filters[0] == "author")
            {
                foreach (Book b in library.books.Books)
                {
                    string[] split = b.author.name.Split(' ');
                    foreach (string s in split)
                    {
                        if (s == filters[1] && !library.bookSearchResults.Contains(b))
                        {
                            library.bookSearchResults.Add(b);
                        }
                    }
                }
            } else if (filters[0] == "category")
            {
                foreach (Book b in library.books.Books)
                {
                    string[] split = b.category.Split(' ');
                    foreach (string s in split)
                    {
                        if (s == filters[1] && !library.bookSearchResults.Contains(b))
                        {
                            library.bookSearchResults.Add(b);
                        }
                    }
                }
            } else
            {

            }
            return RedirectToAction("SearchResults");
        }

        public IActionResult SearchResults(List<Book> books)
        {
            return View(library);
        }

        public bool containsCategory(string category)
        {
            if (library.categoryInfo.Count > 0)
            {
                foreach (string[] c in library.categoryInfo)
                {
                    if (c[0] == category)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public IActionResult CategoryList()
        {
            foreach (Book b in library.books.Books)
            {
                if (!containsCategory(b.category))
                {
                    library.categoryInfo.Add(new string[] {b.category, "" });
                }
            }
            return View(library);
        }

        public IActionResult CreateNewCategory()
        {
            if (library.currentUser.role != "admin" && library.currentUser.role != "librarian")
            {
                library.recentOrder = "Edits cannot be performed by members (only admins or librarians)";
                return RedirectToAction("BookList");
            }
            string[] newCategoryInfo = new string[2];
            return View(newCategoryInfo);
        }

        [HttpPost]
        public IActionResult CreateNewCategory(string[] newCategoryInfo)
        {
            library.addCategory(newCategoryInfo[0], newCategoryInfo[1]);
            return RedirectToAction("CategoryList");
        }

        public IActionResult EditBook()
        {
            string[] bookEdit = new string[8];
            if (library.currentUser.role != "admin" && library.currentUser.role != "librarian")
            {
                library.recentOrder = "Edits cannot be performed by members (only admins or librarians)";
                return RedirectToAction("BookList");
            }
            return View(bookEdit);
        }

        [HttpPost]
        public IActionResult EditBook(string[] bookEdit)
        {
            foreach (Book b in library.books.Books)
            {
                if (b.title == bookEdit[0])
                {
                    b.title = bookEdit[1];
                }
                if (b.author.name == bookEdit[2])
                {
                    b.author.name = bookEdit[3];
                }
                if (b.category == bookEdit[4])
                {
                    b.category = bookEdit[5];
                }
                if (Convert.ToInt32(bookEdit[6]) != null && b.publishDate == Convert.ToInt32(bookEdit[6]))
                {
                    if (Convert.ToInt32(bookEdit[7]) != null)
                    {
                        b.publishDate = Convert.ToInt32(bookEdit[7]);
                    }
                }
            }
            return RedirectToAction("BookList");
        }

        public IActionResult Delete()
        {
            if (library.currentUser.role != "admin" && library.currentUser.role != "librarian")
            {
                library.recentOrder = "Edits cannot be performed by members (only admins or librarians)";
                return RedirectToAction("BookList");
            }
            string[] deleteInfo = new string[3];
            return View(deleteInfo);
        }

        [HttpPost]
        public IActionResult Delete(string[] deleteInfo)
        {
            foreach (Book b in library.books.Books)
            {
                if (deleteInfo[0] == b.title)
                {
                    library.books.Books.Remove(b);
                    break;
                }
            }
            int bookIndex = 0;
            while (bookIndex < library.books.Books.Count)
            {
                for (; bookIndex < library.books.Books.Count; bookIndex++)
                {
                    if (deleteInfo[1] == library.books.Books[bookIndex].author.name || deleteInfo[2] == library.books.Books[bookIndex].category)
                    {
                        library.books.Books.Remove(library.books.Books[bookIndex]);
                    }
                }
            }
            foreach (Author a in library.authors.Authors)
            {
                if (a.name == deleteInfo[1])
                {
                    library.authors.Authors.Remove(a);
                    break;
                }
            }
            foreach (string[] c in library.categoryInfo)
            {
                if (c[0] == deleteInfo[2])
                {
                    library.categoryInfo.Remove(c);
                    break;
                }
            }
            return RedirectToAction("BookList");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}