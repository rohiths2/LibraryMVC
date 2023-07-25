using System.Diagnostics.Contracts;

namespace LibraryMVC.Models
{
    public class User
    {
        public string username;
        public string category;
        public User()
        {
            username = "";
            category = "";
        }
    }
}
