using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels._3PartyShip
{
    public class ShippingOrderRequest
    {
        public required string ToName { get; set; }
        public required string ToPhone { get; set; }
        //public required string ToAddress { get; set; }
        //public required string ToProvince { get; set; }
        //public required string ToDistrict { get; set; }
        //public required string ToWard { get; set; }
        public string Address { get; set; } = string.Empty;
        public required List<ShippingItemDto> Items { get; set; }
    }
}
