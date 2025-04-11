using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Domain.Entities
{
    public class ComplaintDetail : BaseEntity
    {
        public Guid ComplaintId { get; set; }
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual Complaint Complaint { get; set; } = null!;
    }
}
