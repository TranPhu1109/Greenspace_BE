using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.ServiceOrder
{
    public class ServiceOrderUpdateStatusModel
    {
        public int Status { get; set; } = default!;
        public string DeliveryCode { get; set; } = string.Empty;
        public string? ReportManger { get; set; } = string.Empty;
        public string? ReportAccoutant { get; set; } = string.Empty;

    }
}
