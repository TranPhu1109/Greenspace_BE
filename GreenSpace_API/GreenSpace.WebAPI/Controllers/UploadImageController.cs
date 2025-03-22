using GreenSpace.Application.Services;
using GreenSpace.Application.ViewModels.Images;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GreenSpace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CloudinaryController : ControllerBase
    {
        private readonly CloudinaryService _cloudinaryService;

        public CloudinaryController(CloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadImages([FromForm] ImageUploadModel model)
        {
            var responseModel = new ImageCreateModel
            {
                ImageUrl = model.ImageUrl != null ? await _cloudinaryService.UploadImageAsync(model.ImageUrl) : string.Empty,
                Image2 = model.Image2 != null ? await _cloudinaryService.UploadImageAsync(model.Image2) : string.Empty,
                Image3 = model.Image3 != null ? await _cloudinaryService.UploadImageAsync(model.Image3) : string.Empty
            };

            return Ok(responseModel);
        }
    }
}
