using System;
using System.Collections.Generic;
using System.Linq;

namespace Store.Memory
{
    public class BookRepository : IBookRepository
    {
        private readonly List<Book> Books = new List<Book>()
        {
            new Book(1, "Art Of Programming"),
            new Book(2, "Refactoring"),
            new Book(3, "C Programming Language")
        };
        public List<Book> GetByTitle(string titlePart)
        {
            return Books.Where(book => book.Title.Contains(titlePart)).ToList();
        }
    }
}
