using Microsoft.AspNetCore.Http;
using PhoneNumbers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Store.Web.App
{
    public class OrderService
    {
        private readonly IBookRepository bookRepository;
        private readonly IOrderRepository orderRepository;
        private readonly INotificationService notificationService;
        private readonly IHttpContextAccessor httpContextAccessor;

        protected ISession Session => httpContextAccessor.HttpContext.Session;

        public OrderService(IBookRepository bookRepository,
                            IOrderRepository orderRepository,
                            INotificationService notificationService,
                            IHttpContextAccessor httpContextAccessor)
        {
            this.orderRepository = orderRepository;
            this.bookRepository = bookRepository;
            this.notificationService = notificationService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<(bool hasValue, OrderModel model)> TryGetModelAsync()
        {
            var (hasValue, order) = await TryGetOrderAsync();
            if (hasValue)
                return (true, await MapAsync(order));

            return (false, null);
        }

        internal async Task<(bool hasValue, Order order)> TryGetOrderAsync()
        {
            if (Session.TryGetCart(out Cart cart))
            {
                var order = await orderRepository.GetByIdAsync(cart.OrderId);
                return (true, order);
            }

            return (false, null);
        }

        internal async Task<OrderModel> MapAsync(Order order)
        {
            var books = await GetBooksAsync(order);
            var items = from item in order.Items
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
                Items = items.ToArray(),
                TotalCount = order.TotalCount,
                TotalPrice = order.TotalPrice,
                CellPhone = order.CellPhone,
                DeliveryDescription = order.Delivery?.Description,
                PaymentDescription = order.Payment?.Description,
            };
        }

        internal async Task<IEnumerable<Book>> GetBooksAsync(Order order)
        {
            var bookIds = order.Items.Select(item => item.BookId);

            return await bookRepository.GetAllByIdsAsync(bookIds);
        }

        public async Task<OrderModel> AddBookAsync(int bookId, int count)
        {
            if (count < 1)
                throw new InvalidOperationException("Too few books to add.");

            var (hasValue, order) = await TryGetOrderAsync();

            if (!hasValue)
                order = await orderRepository.CreateAsync();

            await AddOrUpdateBookAsync(order, bookId, count);

            UpdateSession(order);

            return await MapAsync(order);
        }

        internal async Task AddOrUpdateBookAsync(Order order, int bookId, int count)
        {
            var book = await bookRepository.GetByIdAsync(bookId);

            if (order.Items.TryGet(bookId, out OrderItem orderItem))
                orderItem.Count += count;
            else
                order.Items.Add(book.Id, book.Price, count);

            await orderRepository.UpdateAsync(order);
        }

        internal void UpdateSession(Order order)
        {
            var cart = new Cart(order.Id, order.TotalCount, order.TotalPrice);
            Session.Set(cart);
        }

        public async Task<OrderModel> UpdateBookAsync(int bookId, int count)
        {
            var order = await GetOrderAsync();
            order.Items.Get(bookId).Count = count;

            await orderRepository.UpdateAsync(order);
            UpdateSession(order);

            return await MapAsync(order);
        }

        public async Task<OrderModel> RemoveBookAsync(int bookId)
        {
            var order = await GetOrderAsync();
            order.Items.Remove(bookId);

            await orderRepository.UpdateAsync(order);
            UpdateSession(order);

            return await MapAsync(order);
        }

        public async Task<Order> GetOrderAsync()
        {
            var (hasValue, order) = await TryGetOrderAsync();
            if (hasValue)
                return order;

            throw new InvalidOperationException("Empty session.");
        }
        public async Task<OrderModel> SendConfirmationAsync(string cellPhone)
        {
            var order = await GetOrderAsync();
            var model = await MapAsync(order);

            try
            {
                var phoneNumber = phoneNumberUtil.Parse(cellPhone, "pl");
                model.CellPhone = phoneNumberUtil.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL);

                var confirmationCode = 1111; // todo: random.Next(1000, 10000) = 1000, 1001, ..., 9998, 9999
                Session.SetInt32(model.CellPhone, confirmationCode);
                await notificationService.SendConfirmationCodeAsync(model.CellPhone, confirmationCode);

            }
            catch (NumberParseException)
            {
                model.Errors["cellPhone"] = "Does not match the format +48123456789";
            }

            return model;
        }

        private readonly PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();

        public async Task<OrderModel> ConfirmCellPhoneAsync(string cellPhone, int confirmationCode)
        {
            int? storedCode = Session.GetInt32(cellPhone);
            var model = new OrderModel();

            if (storedCode == null)
            {
                model.Errors["cellPhone"] = "Something happened. Try to get the code again.";
                return model;
            }

            if (storedCode != confirmationCode)
            {
                model.Errors["confirmationCode"] = "Invalid code. Check and try again.";
                return model;
            }

            var order = await GetOrderAsync();
            order.CellPhone = cellPhone;
            await orderRepository.UpdateAsync(order);

            Session.Remove(cellPhone);

            return await MapAsync(order);
        }

        public async Task<OrderModel> SetDeliveryAsync(OrderDelivery delivery)
        {
            var order = await GetOrderAsync();
            order.Delivery = delivery;
            await orderRepository.UpdateAsync(order);

            return await MapAsync(order);
        }

        public async Task<OrderModel> SetPaymentAsync(OrderPayment payment)
        {
            var order = await GetOrderAsync();
            order.Payment = payment;
            await orderRepository.UpdateAsync(order);
            Session.RemoveCart();

            //notificationService.StartProcess(order); For sending email
            //await notificationService.StartProcessAsync(order); For sending email async

            return await MapAsync(order);
        }
    }
}
