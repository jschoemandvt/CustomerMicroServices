using CustomerMicroServices.FrontEnd.Events;
using CustomerMicroServices.FrontEnd.Models;
using Dapr.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace CustomerMicroServices.FrontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DaprClient _daprClient;

        public HomeController(ILogger<HomeController> logger, DaprClient daprClient)
        {
            _logger = logger;
            _daprClient = daprClient;
        }

        [HttpGet]
        public IActionResult UploadData()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadData(UploadDataCommand model)
        {
            MemoryStream ms = new();
            using (var uploadedFile = model.File.OpenReadStream())
            {
                await uploadedFile.CopyToAsync(ms);
            }

            var imageData = ms.ToArray();
            model.CustomerPhotoUrl = model.File.FileName;
            model.OrderId = Guid.NewGuid();
            var eventData = new OrderReceivedEvent(model.OrderId, model.CustomerPhotoUrl, model.CustomerEmail, imageData);

            try
            {
                //everything is handled behind the scenes so no more conversion to json etc.
                await _daprClient.PublishEventAsync("eventbus", "OrderReceivedEvent", eventData);
                _logger.LogInformation("Pulishing event: OrderReceivedEvent, {OrderId}", model.OrderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR: Pulishing event: OrderReceivedEvent, {OrderId}", model.OrderId);
            }

            ViewData["OrderId"] = model.OrderId;
            return View("Thanks");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}