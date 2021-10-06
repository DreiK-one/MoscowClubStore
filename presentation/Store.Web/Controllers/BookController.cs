using Microsoft.AspNetCore.Mvc;
using Store.Web.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Store.Web.Controllers
{
    public class BookController : Controller
    {
        private readonly BookService bookService;

        public BookController(BookService bookService)
        {
            this.bookService = bookService;
        }
        public IActionResult Index(int id)
        {
            var model = bookService.GetById(id);
            return View(model);
        }
    }
}
