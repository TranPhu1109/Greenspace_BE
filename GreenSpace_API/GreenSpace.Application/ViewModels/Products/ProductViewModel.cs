using GreenSpace.Application.ViewModels.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.Products
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid CategoryId { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid ImageId { get; set; }
        public int Size { get; set; }
        public string CategoryName { get; set; } = default!;
        public string DesignImage1URL { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public ImageCreateModel? Image { get;  set; }
    }
}
