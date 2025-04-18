﻿using GreenSpace.Application.ViewModels.Images;
using GreenSpace.Application.ViewModels.ServiceOrderDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.ServiceOrder
{
    public class ServiceOrderUpdateModel
    {
    
        public int ServiceType { get; set; } = default!;
        public double? DesignPrice { get; set; } = default!;

        public string Description { get; set; } = string.Empty;
        public int   Status { get; set; } = default!;
        public string Report { get; set; } = string.Empty;
        public string? ReportManger { get; set; } = string.Empty;
        public string? ReportAccoutant { get; set; } =  string.Empty ;
        public ImageCreateModel? Image { get; set; }

        public List<ServiceOrderDetailCreateModel> ServiceOrderDetails { get; set; } = new List<ServiceOrderDetailCreateModel>();
    }
}
