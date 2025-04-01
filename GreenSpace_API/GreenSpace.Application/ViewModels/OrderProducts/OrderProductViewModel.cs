﻿using GreenSpace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.OrderProducts
{
    public class OrderProductViewModel
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public double ShipPrice { get; set; }
        public double TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<Product> Products { get; set; } = new();
    }
}
