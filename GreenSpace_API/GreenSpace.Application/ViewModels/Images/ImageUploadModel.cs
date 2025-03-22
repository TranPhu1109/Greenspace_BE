using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.Images
{
    public class ImageUploadModel
    {
        public IFormFile? ImageUrl { get; set; }

     
        public IFormFile? Image2 { get; set; }

        public IFormFile? Image3 { get; set; }
    }
}
