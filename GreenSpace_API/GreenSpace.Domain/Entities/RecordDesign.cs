using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Domain.Entities
{
    public class RecordDesign : BaseEntity
    {
        public int phase { get; set; }
        public bool isSelected { get; set; }
        public Guid ServiceOrderId { get; set; }
        public ServiceOrder? ServiceOrder { get; set; }
        public Guid ImageId { get; set; }
        public Image? Image { get; set; }   
    }
}
