using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Tests
{
    class StubBookRepository : IBookRepository
    {
        public List<Book> ResultOfGetAllByIsbn { get; set; }

        public List<Book> ResultOfGetAllByTitleOrAuthor { get; set; }

        public List<Book> GetAllByIds(IEnumerable<int> bookId)
        {
            throw new NotImplementedException();
        }

        public List<Book> GetAllByIsbn(string isbn)
        {
            return ResultOfGetAllByIsbn;
        }

        public List<Book> GetAllByTitleOrAuthor(string titleOrAuthor)
        {
            return ResultOfGetAllByTitleOrAuthor;
        }

        public Book GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
