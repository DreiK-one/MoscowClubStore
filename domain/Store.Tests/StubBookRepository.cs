using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Tests
{
    class StubBookRepository : IBookRepository
    {
        public Book[] ResultOfGetAllByIsbn { get; set; }

        public Book[] ResultOfGetAllByTitleOrAuthor { get; set; }

        public Book[] GetAllByIds(IEnumerable<int> bookId)
        {
            throw new NotImplementedException();
        }

        public Task<Book[]> GetAllByIdsAsync(IEnumerable<int> bookIds)
        {
            throw new NotImplementedException();
        }

        public Book[] GetAllByIsbn(string isbn)
        {
            return ResultOfGetAllByIsbn;
        }

        public Task<Book[]> GetAllByIsbnAsync(string isbn)
        {
            throw new NotImplementedException();
        }

        public Book[] GetAllByTitleOrAuthor(string titleOrAuthor)
        {
            return ResultOfGetAllByTitleOrAuthor;
        }

        public Task<Book[]> GetAllByTitleOrAuthorAsync(string titleOrAuthor)
        {
            throw new NotImplementedException();
        }

        public Book GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Book> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
