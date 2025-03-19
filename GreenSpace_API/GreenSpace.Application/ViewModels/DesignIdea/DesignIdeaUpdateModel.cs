using GreenSpace.Application.ViewModels.Images;
using GreenSpace.Application.ViewModels.ProductDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.DesignIdea
{
    public class DesignIdeaUpdateModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public double Price { get; set; } = default!;

        public Guid DesignIdeasCategoryId { get; set; }

        public ImageCreateModel Image { get; set; } = new ImageCreateModel();
        public List<ProductDetailCreateModel>? ProductDetails { get; set; } = new List<ProductDetailCreateModel>();
    }
}
