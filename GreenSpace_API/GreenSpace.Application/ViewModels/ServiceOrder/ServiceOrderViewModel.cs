using GreenSpace.Application.ViewModels.Images;
using GreenSpace.Application.ViewModels.ProductDetail;
using GreenSpace.Application.ViewModels.RecordDesign;
using GreenSpace.Application.ViewModels.RecordSketch;
using GreenSpace.Application.ViewModels.ServiceOrderDetail;
using GreenSpace.Application.ViewModels.WorkTasks;
using GreenSpace.Domain.Entities;
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
        public string UserName { get; set; } = string.Empty;
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

        public string Report { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string DeliveryCode { get; set; } = string.Empty;
        //public Guid? RecordDesignId { get; set; } = default!;
        //public Guid? RecordSketchId { get; set; } = default!;
        public string? ReportManger { get; set; } = string.Empty;
        public string? ReportAccoutant { get; set; } = string.Empty;
        public decimal DepositPercentage { get; set; } = 100m;
        public decimal RefundPercentage { get; set; } = 100m;
        public string SkecthReport { get; set; } = string.Empty;
        public DateOnly? ContructionDate { get; set; }
        public TimeOnly? ContructionTime { get; set; }
        public decimal ContructionPrice { get; set; } = default!;
        public DateTime CreationDate { get; set; }
        public ImageCreateModel? Image { get; set; }

        public List<ServiceOrderDetailViewModel> ServiceOrderDetails { get; set; } = new List<ServiceOrderDetailViewModel>();
        public List<WorkTaskViewModel> WorkTasks { get; set; } = new();
        public List<RecordSketchViewModel> RecordSketches { get; set; } = new();
        public List<RecordDesignViewModel> RecordDesigns{ get; set; } = new();
    }
}
