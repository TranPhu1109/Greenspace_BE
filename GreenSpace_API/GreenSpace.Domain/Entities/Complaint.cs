using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Domain.Entities
{
    public class Complaint : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid? ServiceOrderId { get; set; }
        public Guid? OrderId { get; set; }
        public Guid ImageId { get; set; }
        public Guid? ComplaintReasonId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string ComplaintType { get; set; } = string.Empty;
        public int Status { get; set; } = default;
        public string DeliveryCode { get; set; } = string.Empty;
        public User User { get; set; } = default!;
        public ServiceOrder? ServiceOrder { get; set; }
        public Order? Order { get; set; }
        public Image Image { get; set; } = null!;
        public ComplaintReason? ComplaintReason { get; set; }
        public ICollection<ComplaintDetail> ComplaintDetails { get; set; } = new List<ComplaintDetail>();
        
    }
}
