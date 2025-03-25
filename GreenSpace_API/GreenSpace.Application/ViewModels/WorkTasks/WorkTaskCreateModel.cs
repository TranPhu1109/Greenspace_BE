using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.WorkTasks
{
    public class WorkTaskCreateModel
    {
        public Guid ServiceOrderId { get; set; }

        public Guid UserId { get; set; }


        public string Note { get; set; } = string.Empty;
    }
}
