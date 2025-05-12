using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.ComplaintDetail
{
    public class ComplaintDetailUpdateModel
    {
        public Guid ProductId { get; set; }
        public bool IsCheck { get; set; } = false;
    }
}
