using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LibraryMVC.Models
{
    public class Book
    {
        public string title;
        public Author author;
        public string category;
        public int publishDate;
        public int quantity;
        public Book()
        {
            title = "initialized";
            author = new Author();
            category = "";
            DateTime today = DateTime.Now;
            publishDate = (today.Month * 10000) + (today.Day * 100) + (today.Year);
            quantity = 1;
        }

        public Book(string setTitle, string authorName, int authorBD, string setCategory, int setPublishDate, int setQuantity)
        {
            title = setTitle;
            author = new Author(authorName, authorBD, "");
            publishDate = setPublishDate;
            category = setCategory;
            quantity = setQuantity;
        }

    }
}
