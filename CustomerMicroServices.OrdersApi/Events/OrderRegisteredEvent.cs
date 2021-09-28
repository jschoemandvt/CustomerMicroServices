using System;

namespace CustomerMicroServices.OrdersApi.Events
{
    public class OrderRegisteredEvent
    {
        public Guid OrderId { get; set; }
        public string CustomerEmail { get; set; }
        public byte[] ImageData { get; set; }
    }
}