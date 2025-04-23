using GreenSpace.Application.ViewModels.ComplaintDetail;
using GreenSpace.Application.ViewModels.Images;
using GreenSpace.Application.ViewModels.OrderProducts;
using GreenSpace.Application.ViewModels.ProductDetail;
using GreenSpace.Application.ViewModels.ServiceOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.Complaints
{
    public class ComplaintViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? Address { get; set; }
        public string CusPhone { get; set; } = string.Empty;
        public Guid ServiceOrderId { get; set; }
        public Guid OrderId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string ComplaintType { get; set; } = string.Empty;
        public string DeliveryCode { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public ServiceOrderViewModel? ServiceOrder { get; set; } 
        public OrderProductViewModel? OrderProduct { get; set; } 

        public ImageCreateModel? Image { get; set; }
        public List<ComplaintDetailViewModel> ComplaintDetails { get; set; } = new List<ComplaintDetailViewModel>();
    }
}
