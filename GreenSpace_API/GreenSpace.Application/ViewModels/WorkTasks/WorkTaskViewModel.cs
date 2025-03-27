using GreenSpace.Application.ViewModels.ServiceOrder;
using GreenSpace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.WorkTasks
{
    public class WorkTaskViewModel 
    {
        public Guid Id { get; set; }
        public Guid ServiceOrderId { get; set; }
        public Guid UserId { get; set; }
        public string  UserName { get; set; } = string.Empty;

        public string  Status { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public ServiceOrderViewModel ServiceOrder { get; set; } = null!;
    }
}
