using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store
{
    public class OrderItem
    {
        public int BookId { get; }

        private int _count;

        public int Count 
        {
            get { return _count; }
            set 
            {
                ThrowIfInvalidCount(value);
                _count = value;
            } 
        }

        public decimal Price { get; }

        public OrderItem(int bookid, int count, decimal price)
        {
            ThrowIfInvalidCount(count);

            BookId = bookid;
            Count = count;
            Price = price;
        }

        private static void ThrowIfInvalidCount(int count)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException("Count must be greater then 0");
        }
    }
}
