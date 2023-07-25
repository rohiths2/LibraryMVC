namespace LibraryMVC.Models
{
    public class Author
    {
        public string name;
        public int birthdate;
        public string bio;
        public Author()
        {
            name = "";
            birthdate = 010103;
            bio = "";
        }

        public Author(string setName, int setBD, string setBio)
        {
            name = setName;
            birthdate = setBD;
            bio = setBio;
        }
    }
}
