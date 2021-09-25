using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public interface IBookRepository
    {
        List<Book> GetAllByIsbn(string isbn);

        List<Book> GetAllByTitleOrAuthor(string titleOrAuthor);
    }
}
