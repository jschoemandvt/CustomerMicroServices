using Microsoft.AspNetCore.Http;
using System;

namespace CustomerMicroServices.FrontEnd.Models
{
    public class UploadDataCommand
    {
        public Guid OrderId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhotoUrl { get; set; }
        public IFormFile File { get; set; }
    }
}