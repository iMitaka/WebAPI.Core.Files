using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageTest.API.Models
{
    public class FilePostModel
    {
        public IFormFile File { get; set; }
        public string UserId { get; set; }
    }
}
