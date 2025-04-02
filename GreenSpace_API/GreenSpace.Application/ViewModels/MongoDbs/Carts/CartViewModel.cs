using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.MongoDbs.Carts
{
    public class CartViewModel
    {
        public string Id { get; set; } = string.Empty;
        public Guid UserId { get; set; } = Guid.Empty;
        public bool IsCurrent { get; set; } = true;
        public List<CartItemViewModel> Items { get; set; } = new();
    }
}
