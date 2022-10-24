using Bookstore.Models;
using Bookstore.Models.Repositories;
using Bookstore.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
//using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IWebHostEnvironment;

namespace Bookstore.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookstoreRepository<Book> bookRepository;
        private readonly IBookstoreRepository<Author> authorRepository;
        private readonly IHostingEnvironment hosting;
        public BookController(IBookstoreRepository <Book> bookRepository, IBookstoreRepository<Author> authorRepository, IHostingEnvironment hosting)
        {
            this.bookRepository = bookRepository;
            this.authorRepository = authorRepository;
            this.hosting = hosting;
        }
        // GET: BookController
        public ActionResult Index()
        {
            var books = bookRepository.List();
            return View(books);
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
            var books = bookRepository.Find (id);
            return View(books);
        }

        // GET: BookController/Create
        public ActionResult Create()
        {
            var model = new BookAuthorViewModel
            {
                Authors = FillSelectList()  //authorRepository.List().ToList()
            };

            return View(model);
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    //string fileName = UploadFile(model.File) == null? string.Empty: UploadFile(model.File); // Ternary operator
                    string fileName = UploadFile(model.File) ?? string.Empty; // coalescing operator

                    if (model.AuthorId == -1)
                    {
                        ViewBag.Message = "Please select an author from the list!";

                        return View(GetAllAuthors());
                    }

                    var author = authorRepository.Find(model.AuthorId);
                    Book book = new Book
                    {
                        Id = model.BookId,
                        Title = model.Title,
                        Description = model.Description,
                        Author = author,
                        ImageUrl = fileName,
                    };
                    bookRepository.Add(book);

                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }

                ModelState.AddModelError("","You have to fill all the required fields!");

                return View(GetAllAuthors());
            
         
        }

        // GET: BookController/Edit/5
        public ActionResult Edit(int id)
        {
            var book = bookRepository.Find(id);
            var authorId = book.Author == null ? book.Author.Id = 0 : book.Author.Id;

            var viewModel = new BookAuthorViewModel
            {
                BookId = book.Id,
                Title= book.Title,
                Description= book.Description,
                AuthorId = authorId, //book.Author.Id,
                Authors = authorRepository.List().ToList(),
                ImageUrl = book.ImageUrl,
            };

            return View(viewModel);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( BookAuthorViewModel viewModel)
        {
            try
            {
                string fileName = UploadFile(viewModel.File, viewModel.ImageUrl);

                var author = authorRepository.Find(viewModel.AuthorId);
                Book book = new Book
                {
                    Id = viewModel.BookId,
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    Author = author,
                    ImageUrl = fileName,
                };
                bookRepository.Update(viewModel.BookId,book);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: BookController/Delete/5
        public ActionResult Delete(int id)
        {
            var book = bookRepository.Find(id);

            return View(book);
        }

        // POST: BookController/Delete/5    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                bookRepository.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        List<Author> FillSelectList()
        {
            var authors = authorRepository.List().ToList();

            authors.Insert(0,new Author { Id = -1,FullName="--- Please select an author ---"});
            return authors;
        }

        BookAuthorViewModel GetAllAuthors()
        {
            var vmodel = new BookAuthorViewModel
            {
                Authors = FillSelectList()
            };
            return vmodel;
        }

        string UploadFile(IFormFile file)
        {
            if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "uploads");
                string fullPath = Path.Combine(uploads,file.FileName);
                file.CopyTo(new FileStream(fullPath, FileMode.Create));
                return file.FileName;

            }
            return null;
        }

        string UploadFile(IFormFile file,string imageUrl)
        {
            if (file != null)
            {
                string uploads = Path.Combine(hosting.WebRootPath, "uploads");

                string newPath = Path.Combine(uploads, file.FileName);
                // Delete old file
                string OldPath = Path.Combine(uploads, imageUrl);

                if (OldPath != newPath )
                {
                    System.IO.File.Delete(OldPath);
                    //Save the new file
                    file.CopyTo(new FileStream(newPath, FileMode.Create));
                }

                return file.FileName;
            }
            return imageUrl;
        }

        public ActionResult Search(string term)
        {
            var result = bookRepository.Search(term);
            return View("Index",result); // Note: cz the index view return a list of books then we cann use the same view...
        }

    }
}
