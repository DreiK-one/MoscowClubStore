using System.Collections.Generic;


namespace Store
{
    public interface IBookRepository
    {
        List<Book> GetAllByIsbn(string isbn);

        List<Book> GetAllByTitleOrAuthor(string titleOrAuthor);

        Book GetById(int id);
    }
}
