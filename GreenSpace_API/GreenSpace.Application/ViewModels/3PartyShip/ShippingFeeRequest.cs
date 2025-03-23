using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels._3PartyShip
{
    public class ShippingFeeRequest
    {
        public required string ToProvinceName { get; set; }
        public required string ToDistrictName { get; set; }
        public  required string ToWardName { get; set; }
    }
}
