using System.Collections.Generic;

namespace Store.Web.App
{
    public class OrderModel
    {
        public int Id { get; set; }

        public IReadOnlyList<OrderItemModel> Items { get; set; } = new OrderItemModel[0];

        public int TotalCount { get; set; }

        public decimal TotalPrice { get; set; }

        public string CellPhone { get; set; }

        public string DeliveryDiscription { get; set; }

        public string PaymentDiscription { get; set; }

        public Dictionary<string, string> Errors { get; set; } = new Dictionary<string, string>();
    }
}
