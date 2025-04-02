using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Domain.Enum
{
    public enum ComplaintStatusEnum
    {
        pending = 0,
        Approved = 1,
        Processing = 2,
        Delivery = 3,
        refund = 4,
        Complete = 5,
        reject = 6
    }
}
