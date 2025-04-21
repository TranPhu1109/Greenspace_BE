using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.ServiceOrder
{
    public class ServiceOrderUpdateContructorModel
    {
        public DateOnly? ContructionDate { get; set; }
        public TimeOnly? ContructionTime { get; set; }
        public decimal ContructionPrice { get; set; } = default!;
    }
}
