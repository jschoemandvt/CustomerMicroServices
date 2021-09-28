using System;

namespace CustomerMicroServices.OrdersApi.Commands
{
    public class OrderReceivedCommand
    {
        public Guid OrderId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhotoUrl { get; set; }
        public byte[] ImageData { get; set; }
    }
}