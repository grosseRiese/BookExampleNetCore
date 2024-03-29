﻿namespace Bookstore.Models.Repositories
{
    public class BookRepository : IBookstoreRepository<Book>
    {
        List<Book> books;
        public BookRepository()
        {
            books = new List<Book>()
            {
                new Book
                {
                    Id=1,
                    Title="C# Programming",
                    Description="No description", 
                    Author = new Author{ Id = 2 },
                    ImageUrl = "img1.jpg"
                },
                new Book
                {
                    Id=2,Title="Java Programming",Description="No data",Author = new Author(),ImageUrl = "img4.jpg"
                },
                new Book
                {
                    Id=3,Title="Python Programming",Description="No information",Author = new Author(), ImageUrl = "img3.png"
                },
                new Book
                {
                    Id=4,Title="VueJS Programming",Description="Nothing",Author = new Author(), ImageUrl = "img2.png"
                },
            };
        }
        public void Add(Book entity)
        {
            entity.Id = books.Max(b => b.Id) +1;
            books.Add(entity);
        }

        public void Delete(int id)
        {
            var book = Find(id); //books.SingleOrDefault(b => b.Id == id);
            books.Remove(book);
        }

        public Book Find(int id)
        {
            var book = books.SingleOrDefault(b => b.Id == id);
            return book;
        }

        public IList<Book> List()
        {
            return books;
        }

        public List<Book> Search(string term)
        {
            return books.Where(a => a.Title.Contains(term)).ToList();
        }

        public void Update(int id,Book newBook)
        {
            var book = Find(id); // books.SingleOrDefault(b => b.Id == id);
            book.Title = newBook.Title;
            book.Description = newBook.Description; 
            book.Author = newBook.Author;   
            book.ImageUrl = newBook.ImageUrl;

        }
    }
}
