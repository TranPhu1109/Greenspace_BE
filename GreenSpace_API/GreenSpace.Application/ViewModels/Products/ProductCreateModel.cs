using GreenSpace.Application.ViewModels.Images;
using GreenSpace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.Products
{
    public class ProductCreateModel
    {
        public string Name { get; set; } = string.Empty;

        public Guid CategoryId { get; set; } 
        public double Price { get; set; }

        public int Stock { get; set; }

        public string Description { get; set; } = string.Empty;


        public int Size { get; set; }

        public ImageCreateModel Image  { get; set; } = new ImageCreateModel();
    }
}
