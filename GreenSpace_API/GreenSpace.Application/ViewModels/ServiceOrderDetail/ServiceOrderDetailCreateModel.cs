﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.ServiceOrderDetail
{
    public class ServiceOrderDetailCreateModel
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

    }
}
