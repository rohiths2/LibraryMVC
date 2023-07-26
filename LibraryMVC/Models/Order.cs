using System.ComponentModel;

namespace LibraryMVC.Models
{
    public class Order
    {
        public string name;
        public string bookTitle;
        public DateTime dueDate;
        public string returned;

        public Order() {
            name = "";
            bookTitle = "book";
            dueDate = DateTime.Now.AddDays(7);
            returned = "Due in 7 days";
        }

        public Order(string user_, string bookTitle_, DateTime dueDate_)
        {
            this.name = user_;
            this.bookTitle = bookTitle_;
            this.dueDate = dueDate_;
            returned = "Due in 7 days";
        }
    }
}
