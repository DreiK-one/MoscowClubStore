using System;
using System.Collections.Generic;
using System.Linq;

namespace Store.Memory
{
    public class BookRepository : IBookRepository
    {
        private readonly List<Book> Books = new List<Book>()
        {
            new Book(1, "ISBN 12312-31231", "D. Knuth", "Art Of Programming"),
            new Book(2, "ISBN 12312-31232", "M. Fowler","Refactoring"),
            new Book(3, "ISBN 12312-31232", "B. Kernighan, D. Ritchie", "C Programming Language")
        };

        public List<Book> GetAllByIsbn(string isbn)
        {
            return Books.Where(book => book.Isbn == isbn).ToList();
        }

        public List<Book> GetAllByTitleOrAuthor(string query)
        {
            return Books.Where(book => book.Author.Contains(query)
                                    || book.Title.Contains(query))
                .ToList();
        }
    }
}
