using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Store.Tests
{
    public class BookServiceTests
    {
        [Fact]
        public void MockGetAllByQuery_WithIsbn_CallsGetAllByTitleOrAuthor() //With Moq
        {
            var bookRepositoryStub = new Mock<IBookRepository>();
            bookRepositoryStub.Setup(x => x.GetAllByIsbn("Ritchie")) // Example
                .Returns(new List<Book> { new Book(1, "", "", "", "", 0m) });
            bookRepositoryStub.Setup(x => x.GetAllByTitleOrAuthor(It.IsAny<string>())) //Any string
                .Returns(new List<Book> { new Book(2, "", "", "", "", 0m) });

            var bookService = new BookService(bookRepositoryStub.Object);
            var author = "Ritchie";

            var actual = bookService.GetAllByQuery(author);

            Assert.Collection(actual, book => Assert.Equal(2, book.Id));
        }


        [Fact]
        public void GetAllByQuery_WithAuthor_CallsGetAllByIsbn() //With Moq
        {
            var bookRepositoryStub = new Mock<IBookRepository>();
            bookRepositoryStub.Setup(x => x.GetAllByIsbn(It.IsAny<string>()))
                .Returns(new List<Book> { new Book(1, "", "", "", "", 0m) });
            bookRepositoryStub.Setup(x => x.GetAllByTitleOrAuthor(It.IsAny<string>()))
                .Returns(new List<Book> { new Book(2, "", "", "", "", 0m) });

            var bookService = new BookService(bookRepositoryStub.Object);
            var validIsbn = "ISBN 12345-67890";

            var actual = bookService.GetAllByQuery(validIsbn);

            Assert.Collection(actual, book => Assert.Equal(1, book.Id));
        }

        [Fact]
        public void GetAllByQuery_WithIsbn_CallsGetAllByIsbn() //like a Moq (handmade)
        {
            const int idOfIsbnSearch = 1;
            const int idOfAuthorSearch = 2;

            var bookRepository = new StubBookRepository();

            bookRepository.ResultOfGetAllByIsbn = new List<Book> 
            { 
                new Book(idOfIsbnSearch, "", "", "", "", 0m)
            };

            bookRepository.ResultOfGetAllByTitleOrAuthor = new List<Book>
            {
                new Book(idOfAuthorSearch, "", "", "", "", 0m)
            };


            var bookService = new BookService(bookRepository);

            var books = bookService.GetAllByQuery("ISBN 12345-67890");

            Assert.Collection(books, book => Assert.Equal(idOfIsbnSearch, book.Id));
        }

        [Fact]
        public void GetAllByQuery_WithIsbn_CallsGetAllByTitleOrAuthor() //like a Moq (handmade)
        {
            const int idOfIsbnSearch = 1;
            const int idOfAuthorSearch = 2;

            var bookRepository = new StubBookRepository();

            bookRepository.ResultOfGetAllByIsbn = new List<Book>
            {
                new Book(idOfIsbnSearch, "", "", "", "", 0m)
            };

            bookRepository.ResultOfGetAllByTitleOrAuthor = new List<Book>
            {
                new Book(idOfAuthorSearch, "", "", "", "", 0m)
            };


            var bookService = new BookService(bookRepository);

            var books = bookService.GetAllByQuery("Programming");

            Assert.Collection(books, book => Assert.Equal(idOfAuthorSearch, book.Id));
        }
    }
}
