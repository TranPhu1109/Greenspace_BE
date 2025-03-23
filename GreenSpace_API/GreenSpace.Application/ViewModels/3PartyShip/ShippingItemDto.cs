﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels._3PartyShip
{
    public class ShippingItemDto
    {
        public required string Name { get; set; }
        public required string Code { get; set; }
        public required int Quantity { get; set; }
    }
}
