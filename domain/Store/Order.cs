using System.Collections.Generic;
using System.Linq;
using System;

namespace Store
{
    public class Order
    {
        public int Id { get; }

        private List<OrderItem> _items; 

        public IReadOnlyCollection<OrderItem> Items 
        {
            get { return _items; }                
        }

        public OrderState State { get; private set; }

        public string CellPhone { get; set; }

        public OrderDelivery Delivery { get; set; }

        public OrderPayment Payment { get; set; }

        public int TotalCount => _items.Sum(item => item.Count); 

        public decimal TotalPrice => _items.Sum(item => item.Price * item.Count) + (Delivery?.Amount ?? 0m);


        public Order(int id, OrderState state, IEnumerable<OrderItem> items)
        {
            Id = id;
            State = state;
            _items = new List<OrderItem>(items);
        }

        public void AddItem(Book book, int count)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            ValidateState(OrderState.Created);

            var item = _items.SingleOrDefault(x => x.BookId == book.Id);

            if (item != null)
            {
                _items.Add(new OrderItem(book.Id, item.Count + count, book.Price ));
                _items.Remove(item);
            }
            else
                _items.Add(new OrderItem(book.Id, count, book.Price));
        }

        public void StartProcess()
        {
            ValidateState(OrderState.Created);

            State = OrderState.ProcessStarted;
        }

        private void ValidateState(OrderState state)
        {
            if (State != state)
                throw new InvalidOperationException("Invalid state.");
        }


        public OrderItem GetItem(int bookId)
        {
            int index = _items.FindIndex(item => item.BookId == bookId);

            if (index == -1)
                ThrowBookException("Book not found.", bookId);

            return _items[index];
        }

        public void AddOrUpdateItem(Book book, int count)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            int index = _items.FindIndex(item => item.BookId == book.Id);

            if (index == -1)
                _items.Add(new OrderItem(book.Id, count, book.Price));
            else
                _items[index].Count += count;
        }

        public void RemoveItem(int bookId)
        {
            int index = _items.FindIndex(item => item.BookId == bookId);

            if (index == -1)
                ThrowBookException("Order does not contain specified item.", bookId);

            _items.RemoveAt(index);
        }

        private void ThrowBookException(string message, int bookId)
        {
            var exception = new InvalidOperationException(message);

            exception.Data["BookId"] = bookId;
 
            throw exception;
        }
    }
}
