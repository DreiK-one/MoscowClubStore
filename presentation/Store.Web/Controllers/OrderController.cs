using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.Web.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Store;

namespace Store.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBookRepository bookRepository;
        private readonly IOrderRepository orderRepository;
        private readonly INotificationService notificationService;

        public OrderController(IBookRepository bookRepository, IOrderRepository orderRepository, INotificationService notificationService)
        {
            this.orderRepository = orderRepository;
            this.bookRepository = bookRepository;
            this.notificationService = notificationService;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.TryGetCart(out Cart cart))
            {
                var order = orderRepository.GetById(cart.OrderId);
                var model = Map(order);

                return View(model);
            }
            return View("Empty");
        }

        private OrderModel Map(Order order)
        {
            var bookIds = order.Items.Select(item => item.BookId);
            var books = bookRepository.GetAllByIds(bookIds);
            var itemModels = from item in order.Items
                             join book in books on item.BookId equals book.Id
                             select new OrderItemModel
                             {
                                 BookId = book.Id,
                                 Title = book.Title,
                                 Author = book.Author,
                                 Price = item.Price,
                                 Count = item.Count,
                             };
            return new OrderModel
            {
                Id = order.Id,
                State = order.State,
                Items = itemModels.ToArray(),
                TotalCount = order.TotalCount,
                TotalAmount = order.TotalAmount,
            };
        }


        [HttpPost]
        public IActionResult AddItem(int bookId, int count = 1)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();
            var book = bookRepository.GetById(bookId);

            order.AddOrUpdateItem(book, count);

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Book", new { id = bookId});

        }

        [HttpPost]
        public IActionResult UpdateItem(int bookId, int count)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            order.GetItem(bookId).Count = count;

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Order");
        }

        [HttpPost]
        public IActionResult RemoveItem(int bookId)
        {
            (Order order, Cart cart) = GetOrCreateOrderAndCart();

            order.RemoveItem(bookId);

            SaveOrderAndCart(order, cart);

            return RedirectToAction("Index", "Order");
        }

        private (Order order, Cart cart) GetOrCreateOrderAndCart()
        {
            Order order;
                if (HttpContext.Session.TryGetCart(out Cart cart))
                {
                    order = orderRepository.GetById(cart.OrderId);
                }
                else
                {
                    order = orderRepository.Create();
                    cart = new Cart(order.Id);
                }
            return (order, cart);
        }

        private void SaveOrderAndCart(Order order, Cart cart)
        {
            orderRepository.Update(order);

            cart.TotalCount = order.TotalCount;
            cart.TotalAmount = order.TotalAmount;

            HttpContext.Session.Set(cart);
        }


        [HttpPost]
        public IActionResult StartProcess(int id)
        {
            var order = orderRepository.GetById(id);
            order.StartProcess();
            orderRepository.Update(order);

            var model = Map(order);

            return View(model);
        }

        [HttpPost]
        public IActionResult SendConfirmation(int id, string cellPhone)
        {
            var order = orderRepository.GetById(id);
            var model = Map(order);

            if (!IsValidCellPhone(cellPhone))
            {
                model.Errors["cellPhone"] = "Empty or does not match the format +48123456789";
                return View("StartProcess", model);
            }

            var code = GenerateCode();
            HttpContext.Session.SetInt32(cellPhone, code);
            notificationService.SendConfirmationCode(cellPhone, code);
            model.CellPhone = cellPhone;

            return View(model);
        }

        private bool IsValidCellPhone(string cellPhone)
        {
            cellPhone = cellPhone?.Replace(" ", "")
                                 ?.Replace("-", "");

            return Regex.IsMatch(cellPhone, @"^\+?\d{11}$");
        }

        private int GenerateCode()
        {
            var random = new Random();
            return random.Next(1, 10000);
        }

        [HttpPost]
        public IActionResult ConfirmCellPhone(int id, string cellPhone, int code)
        {
            var order = orderRepository.GetById(id);
            var model = Map(order);

            var storedCode = HttpContext.Session.GetInt32(cellPhone);
            if (storedCode == null)
                return View("StartProcess", model);

            if (storedCode != code)
            {
                model.Errors["code"] = "Differs from the one sent";
                return View("SendConfirmation", model);
            }

            order.CellPhone = cellPhone;
            orderRepository.Update(order);

            HttpContext.Session.Remove(cellPhone);

            return View(model);
        }
    }
}
