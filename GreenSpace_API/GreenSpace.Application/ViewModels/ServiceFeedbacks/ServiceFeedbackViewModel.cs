using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.ServiceFeedbacks
{
    public class ServiceFeedbackViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string DesignName { get; set; } = string.Empty;
        public int? Rating { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Reply { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ModificationDate { get; set; }
    }
}
