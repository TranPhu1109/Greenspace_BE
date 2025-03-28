using GreenSpace.Application.ViewModels.Images;
using GreenSpace.Application.ViewModels.ProductDetail;
using GreenSpace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.DesignIdea
{
    public class DesignIdeaViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public double DesignPrice { get; set; } = default!;
        public double MaterialPrice { get; set; } = default!;
        public double TotalPrice { get; set; } = default!;
        public string DesignImage1URL { get; set; } = string.Empty;
        public string DesignImage2URL { get; set; } = string.Empty;
        public string DesignImage3URL { get; set; } = string.Empty;
        public Guid ImageId { get; set; }

        public Guid DesignIdeasCategoryId { get; set; }

        public string CategoryName { get; set; } = default!;
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public ImageCreateModel? Image { get; set; }

        public List<ProductDetailViewModel> ProductDetails { get; set; } = new List<ProductDetailViewModel>();
    }
}
