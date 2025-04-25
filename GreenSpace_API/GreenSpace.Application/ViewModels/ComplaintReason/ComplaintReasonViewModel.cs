using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.ComplaintReason
{
    public class ComplaintReasonViewModel
    {
        public Guid Id { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime? ModificationDate { get; set; } = null;
    }
}
