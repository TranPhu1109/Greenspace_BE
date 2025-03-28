using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.WorkTasks
{
    public class WorkTaskUpdateModel
    {
 
        public Guid ServiceOrderId { get; set; }

        public Guid UserId { get; set; }

        public int Status { get; set; } = default!;
        public string Note { get; set; } = string.Empty;
    }
}
