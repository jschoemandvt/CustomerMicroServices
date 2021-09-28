using System;

namespace CustomerMicroServices.FrontEnd.Events
{
    public class OrderReceivedEvent
    {
        public OrderReceivedEvent(Guid orderId, string customerEmail, string customerPhotoUrl, byte[] imageData)
        {
            OrderId = orderId;
            CustomerEmail = customerEmail;
            CustomerPhotoUrl = customerPhotoUrl;
            ImageData = imageData;
        }

        public Guid OrderId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhotoUrl { get; set; }
        public byte[] ImageData { get; set; }
    }
}