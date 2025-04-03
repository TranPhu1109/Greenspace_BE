using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels._3PartyShip
{
    public class ShippingFeeRequest
    {
        public required int ToProvinceName { get; set; }
        public required int ToDistrictName { get; set; }
        public  required string ToWardName { get; set; }
    }
}
