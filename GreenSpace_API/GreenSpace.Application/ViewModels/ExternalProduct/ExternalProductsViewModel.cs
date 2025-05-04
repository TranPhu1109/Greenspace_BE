using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.ExternalProduct
{
    public class ExternalProductsViewModel
    {
        public Guid Id { get; set; }
        public Guid ServiceOrderId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }

        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public bool IsSell { get; set; } = false;
    }
}
