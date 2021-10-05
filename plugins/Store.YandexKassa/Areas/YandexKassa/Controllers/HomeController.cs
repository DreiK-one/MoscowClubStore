using Microsoft.AspNetCore.Mvc;

namespace Store.YandexKassa.Area.YandexKassa.Controllers
{
    [Area("YandexKassa")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        //YandexKassa/Home/Callback
        public IActionResult Callback()
        {
            return View();
        }
    }
}
