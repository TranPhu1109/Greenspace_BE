using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.Complaints
{
    public class ComplaintUpdateModel
    {
        public int Status { get; set; } = default;
        public int ComplaintType { get; set; } = default!;
    }
}
