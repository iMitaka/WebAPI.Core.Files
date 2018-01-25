using ImageTest.API.Models;
using ImageTest.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageTest.API.Controllers
{
    [Route("[controller]/[action]")]
    public class ImagesController : Controller
    {
        private IFileService fileUpload;

        public ImagesController(IFileService fileUpload)
        {
            this.fileUpload = fileUpload;
        }

        [HttpGet("{imagePath}")]
        public IActionResult Get(string imagePath)
        {
            var image = fileUpload.GetImageFile(imagePath);

            if (image != null)
            {
                return File(image.File, "image/" + image.FileType);
            }

            return NotFound("File not found!");
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> PostAsync([FromForm]FilePostModel model)
        {
            var result = await fileUpload.GenerateUserAvatarSource(model);

            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest("File required");
        }

       
    }
}
