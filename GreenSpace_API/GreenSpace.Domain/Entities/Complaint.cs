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
        public Guid ServiceOrderId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ImageId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

        public User User { get; set; } = default!;
        public ServiceOrder? ServiceOrder { get; set; } = null!;
        public Order? Order { get; set; } = null!;
        public Image Image { get; set; } = null!;
    }
}
