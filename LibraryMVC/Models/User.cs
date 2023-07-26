using System.Diagnostics.Contracts;

namespace LibraryMVC.Models
{
    public class User
    {
        public string username;
        public enum category { admin, librarian, member };

        public string password;

        public string role;
        public User()
        {
            username = "guest";
            role = "member";
        }

        public User(string username_, string status)
        {
            username = username_;
            role = status;
        }
    }
}
