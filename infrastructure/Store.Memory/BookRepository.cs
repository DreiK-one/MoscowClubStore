using System;
using System.Collections.Generic;
using System.Linq;

namespace Store.Memory
{
    public class BookRepository : IBookRepository
    {
        private readonly List<Book> books = new List<Book>()
        {
            new Book(1, "ISBN 12312-31231", "D. Knuth", "Art Of Programming", 
                "The Art of Computer Programming (TAOCP) is a comprehensive monograph written by computer scientist Donald Knuth that covers many kinds of programming algorithms and their analysis.", 
                7.19m),
            new Book(2, "ISBN 12312-31232", "M. Fowler","Refactoring",
                "As the application of object technology--particularly the Java programming language--has become commonplace, a new problem has emerged to confront the software development community.",
                12.45m),
            new Book(3, "ISBN 12312-31233", "B. Kernighan, D. Ritchie", "C Programming Language",
                "The C programming language is a computer programming language that was developed to do system programming for the operating system UNIX and is an imperative programming language",
                14.98m)
        };

        public List<Book> GetAllByIds(IEnumerable<int> bookIds)
        {
            var foundBooks = from book in books
                             join bookId in bookIds on book.Id equals bookId
                             select book;
            return foundBooks.ToList();
        }

        public List<Book> GetAllByIsbn(string isbn)
        {
            return books.Where(book => book.Isbn == isbn).ToList();
        }

        public List<Book> GetAllByTitleOrAuthor(string query)
        {
            return books.Where(book => book.Author.Contains(query)
                                    || book.Title.Contains(query))
                .ToList();
        }

        public Book GetById(int id)
        {
            return books.Single(book => book.Id == id);
        }
    }
}
