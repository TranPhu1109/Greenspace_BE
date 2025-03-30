using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.MongoDbs.Carts
{
    public class CartItemViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public int MaxQuantity { get; set; }
        public decimal ActualPrice { get; set; }
        public string ImageURL { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
    }
}
