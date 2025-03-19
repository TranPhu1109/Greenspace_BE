using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.ServiceFeedbacks
{
    public class ServiceFeedbackCreateModel
    {
        public Guid UserId { get; set; }

        public Guid DesignIdeaId { get; set; }

        public int? Rating { get; set; } = null;

        public string Description { get; set; } = string.Empty;
    }
}
