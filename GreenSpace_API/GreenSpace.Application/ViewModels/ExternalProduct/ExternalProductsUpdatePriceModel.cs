using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.ExternalProduct
{
    public class ExternalProductsUpdatePriceModel
    {
        public decimal Price { get; set; }
        public bool IsSell { get; set; } = false;
    }
}
