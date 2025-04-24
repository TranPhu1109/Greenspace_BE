using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Domain.Entities
{
    public class ComplaintReason : BaseEntity
    {
        public string Reason { get; set; } = string.Empty;
    }
}
