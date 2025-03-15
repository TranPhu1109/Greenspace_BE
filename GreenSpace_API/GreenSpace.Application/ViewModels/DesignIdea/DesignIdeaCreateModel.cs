using GreenSpace.Application.ViewModels.Images;
using GreenSpace.Application.ViewModels.ProductDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.DesignIdea
{
    public class DesignIdeaCreateModel
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public double Price { get; set; } = default!;

        public Guid CategoryId { get; set; }

        public ImageCreateModel Image { get; set; } = new ImageCreateModel();
        public List<ProductDetailCreateModel>? ProductDetails { get; set; } = new List<ProductDetailCreateModel>();
    }
}
