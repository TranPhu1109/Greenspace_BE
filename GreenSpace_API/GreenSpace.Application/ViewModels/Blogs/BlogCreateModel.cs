using GreenSpace.Application.ViewModels.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.Blogs
{
    public class BlogCreateModel
    {
        public string Author { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public ImageCreateModel Image { get; set; } = new ImageCreateModel();
    }
}
