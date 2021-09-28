using System;
using System.Collections.Generic;

namespace CustomerMicroServices.OrdersApi.Models
{
    public class Order
    {
        public Order()
        {
            OrderDetails = new List<OrderDetail>();
        }

        public Guid OrderId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhotoUrl { get; set; }
        public byte[] ImageData { get; set; }
        public Status Status { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}