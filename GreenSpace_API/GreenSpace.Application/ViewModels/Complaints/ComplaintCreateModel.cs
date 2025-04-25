using GreenSpace.Application.ViewModels.ComplaintDetail;
using GreenSpace.Application.ViewModels.Images;
using GreenSpace.Application.ViewModels.ServiceOrderDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.Complaints
{
    public class ComplaintCreateModel
    {

        public Guid UserId { get; set; }
        public Guid? ServiceOrderId { get; set; } = default!;

        public Guid? OrderId { get; set; } = default!;
        public int ComplaintType { get; set; } = default!;
        public Guid? ComplaintReasonId { get; set; }
        public string Reason { get; set; } = string.Empty;


        public ImageCreateModel Image { get; set; } = new ImageCreateModel();

        public List<ComplaintDetailCreateModel> ComplaintDetails { get; set; } = new List<ComplaintDetailCreateModel>();
    }
}
