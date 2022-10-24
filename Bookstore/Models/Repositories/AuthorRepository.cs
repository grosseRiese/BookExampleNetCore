namespace Bookstore.Models.Repositories
{
    public class AuthorRepository : IBookstoreRepository<Author>
    {
        IList<Author> _authors;
        public AuthorRepository()
        {
            _authors = new List<Author>()
            {
                new Author
                {
                    Id = 1, FullName = "Sam Andersson"
                },
                new Author
                {
                    Id = 2, FullName = "Zara Larsson"
                },
                new Author
                {
                    Id = 3, FullName = "Mohammed Ayoub"
                },
            };
        }
        public void Add(Author entity)
        {
            entity.Id = _authors.Max(a => a.Id) + 1;
            _authors.Add(entity);
        }

        public void Delete(int id)
        {
            var author = Find(id);
            _authors.Remove(author);
        }

        public Author Find(int id)
        {
            var author = _authors.SingleOrDefault(a => a.Id == id);
            return author;
        }

            public IList<Author> List()
        {
            return _authors;
        }

        public List<Author> Search(string term)
        {
            return _authors.Where(a=>a.FullName.Contains(term)).ToList();
        }

        public void Update(int id, Author newAuthor)
        {
            var author = Find(id); 
            author.FullName = newAuthor.FullName;
        }
    }
}
