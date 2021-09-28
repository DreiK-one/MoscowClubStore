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

        public int TotalCount
        {
            get { return _items.Sum(item => item.Count); }
        }

        public decimal TotalPrice
        {
            get { return _items.Sum(item => item.Price * item.Count); }
        }


        public Order(int id, IEnumerable<OrderItem> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            Id = id;
            _items = new List<OrderItem>(items);
        }

        public void AddItem(Book book, int count)
        {
            if (book == null)
                throw new ArgumentNullException(nameof(book));

            var item = Items.SingleOrDefault(x => x.BookId == book.Id);

            if (item == null)
            {
                _items.Add(new OrderItem(book.Id, count, book.Price));
            }
            else
            {
                _items.Remove(item);
                _items.Add(new OrderItem(book.Id, item.Count + count, book.Price));
            }
        }
    }
}
