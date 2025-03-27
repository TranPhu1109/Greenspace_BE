using GreenSpace.Application.ViewModels.Images;
using GreenSpace.Application.ViewModels.ProductDetail;
using GreenSpace.Application.ViewModels.ServiceOrderDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.ServiceOrder
{
    public class ServiceOrderViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string?  UserName { get; set; }
        public string? Email { get; set; }

        public string Address { get; set; } = string.Empty;

        public string CusPhone { get; set; } = string.Empty;
        public Guid? DesignIdeaId { get; set; }

        public double? Length { get; set; } = default!;
        public double? Width { get; set; } = default!;
        public string ServiceType { get; set; } = string.Empty;
        public double TotalCost { get; set; } = default!;

        public bool IsCustom { get; set; } = false;

        public double? DesignPrice { get; set; } = default!;

        public double? MaterialPrice { get; set; } = default!;

        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public ImageCreateModel? Image { get; set; }

        public List<ServiceOrderDetailViewModel> ServiceOrderDetails { get; set; } = new List<ServiceOrderDetailViewModel>();
    }
}
