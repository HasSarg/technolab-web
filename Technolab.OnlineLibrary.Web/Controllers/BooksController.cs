using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Technolab.OnlineLibrary.Web.Models;
using Technolab.OnlineLibrary.Web.ViewModels;
using static System.Reflection.Metadata.BlobBuilder;

namespace Technolab.OnlineLibrary.Web.Controllers
{
    [AllowAnonymous]
    public class BooksController : Controller
    {
        public BooksController(ILibraryDbContextFactory contextFactory)
        {
            this.ContextFactory = contextFactory;
        }

        public IActionResult Index(string searchTerm)
        {
            var books = Search(searchTerm);

            var model = new BookSearchViewModel
            {
                Books = books.Select(x => new BookViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Author = x.Author
                }).ToList()
            };

            return View(model);
        }

        public ILibraryDbContextFactory ContextFactory { get; }

        [NonAction]
        public List<Book> Search(string searchTerm)
        {
            using var context = ContextFactory.Create();

            var books = (IEnumerable<Book>)context.Books;
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchTerms = searchTerm.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                books = books.Where(book => searchTerms.Any(term => book.Title.Contains(term) || book.Author.Contains(term)));
            }
            return books.ToList();
        }
    }
}
