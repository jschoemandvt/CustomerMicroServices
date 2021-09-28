using CustomerMicroServices.OrdersApi.Commands;
using CustomerMicroServices.OrdersApi.Events;
using CustomerMicroServices.OrdersApi.Models;
using CustomerMicroServices.OrdersApi.Persistence;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace CustomerMicroServices.OrdersApi.Controllers
{
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly IOrderRepository _ordersRepository;
        private readonly DaprClient _daprClient;

        public OrdersController(ILogger<OrdersController> logger, IOrderRepository ordersRepository, DaprClient daprClient)
        {
            _logger = logger;
            _ordersRepository = ordersRepository;
            _daprClient = daprClient;
        }

        [Route("OrderReceived")]
        [HttpPost]
        [Topic("eventbus", "OrderReceivedEvent")]
        public async Task<IActionResult> OrderRecieved(OrderReceivedCommand command)
        {
            if (command?.OrderId != null && command?.CustomerPhotoUrl != null
                                         && command?.CustomerEmail != null && command?.ImageData != null)
            {
                _logger.LogInformation($"Cloud Event {command.OrderId} {command.CustomerEmail} received");
                Image img = Image.Load(command.ImageData);
                await img.SaveAsync(System.DateTime.UtcNow.ToString("yyyyMMddhhmmss.jpg"), new JpegEncoder());
                var order = new Order()
                {
                    OrderId = command.OrderId,
                    ImageData = command.ImageData,
                    CustomerEmail = command.CustomerEmail,
                    CustomerPhotoUrl = command.CustomerPhotoUrl,
                    Status = Status.Registered,
                    OrderDetails = new List<OrderDetail>()
                };

                var orderDetails = await _ordersRepository.GetOrderAsync(order.OrderId);
                if (orderDetails == null)
                {
                    await _ordersRepository.RegisterOrder(order);
                    var orderRegisterEvent = new OrderRegisteredEvent()
                    {
                        OrderId = command.OrderId,
                        ImageData = command.ImageData,
                        CustomerEmail = command.CustomerEmail
                    };

                    await _daprClient.PublishEventAsync("eventbus", "OrderRegisteredEvent", orderRegisterEvent);
                    _logger.LogInformation($"For {command.OrderId}, OrderRegisteredEvent published");
                }

                return Ok();
            }

            return BadRequest();
        }
    }
}