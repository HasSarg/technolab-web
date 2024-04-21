using Microsoft.VisualStudio.TestTools.UnitTesting;
using Technolab.OnlineLibrary.Web.Controllers;
using Technolab.OnlineLibrary.Web.Models;
using Moq;

namespace Technolab.OnlineLibrary.Web.Tests.Controllers
{
    [TestClass]
    public class BooksControllerTests
    {
        [TestMethod]
        public void SingleToken_MatchingTitle()
        {
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Title", Author = "Author", Count = 1 }
            };

            var controller = new BooksController(CreateMockContextFactory(books));

            var result = controller.Search("Title");

            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void SingleToken_MatchingTitle_DiffentCasing()
        {
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Title", Author = "Author", Count = 1 }
            };

            var controller = new BooksController(CreateMockContextFactory(books));

            var result = controller.Search("title");

            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void SingleToken_MatchingAuthor()
        {
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Title", Author = "Author", Count = 1 }
            };

            var controller = new BooksController(CreateMockContextFactory(books));

            var result = controller.Search("Author");

            Assert.IsTrue(result.Any());
        }

        [TestMethod]
        public void SingleToken_Invalid()
        {
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Title", Author = "Author", Count = 1 }
            };

            var controller = new BooksController(CreateMockContextFactory(books));

            var result = controller.Search("Test");

            Assert.IsFalse(result.Any());
        }

        private ILibraryDbContextFactory CreateMockContextFactory(List<Book> books)
        {
            var contextMock = new Mock<ILibraryDbContext>();
            contextMock.Setup(x => x.Books).Returns(books);

            var contextFactoryMock = new Mock<ILibraryDbContextFactory>();

            contextFactoryMock.Setup(x => x.Create()).Returns(contextMock.Object);

            return contextFactoryMock.Object;
        }
    }
}