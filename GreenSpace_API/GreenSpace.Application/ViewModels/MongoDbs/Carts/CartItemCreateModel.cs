﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.MongoDbs.Carts
{
    public class CartItemCreateModel
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
