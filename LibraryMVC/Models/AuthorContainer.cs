namespace LibraryMVC.Models
{
    public class AuthorContainer
    {
        public AuthorContainer()
        {
            Authors = new List<Author>();
        }
        public List<Author> Authors = new List<Author>();
        public void Add(Author a) { Authors.Add(a); }
        public void Remove(Author a) { Authors.Remove(a); }
        public Author Get(string name)
        {
            if (Authors.Count != 0)
            {
                foreach (Author a in Authors)
                {
                    if (a.name == name)
                    {
                        return a;
                    }
                }
                return null;
            }
            return null;
        }
    }
}
