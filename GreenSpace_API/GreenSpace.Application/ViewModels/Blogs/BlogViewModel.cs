using GreenSpace.Application.ViewModels.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.Blogs
{
    public class BlogViewModel
    {
        public Guid Id { get; set; }
        public string Author { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }

        public ImageCreateModel? Image { get; set; }
    }
}
