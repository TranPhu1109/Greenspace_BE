using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Domain.Enum
{
    public enum OrderProductStatus
    {
        Pending = 0,                   // Chờ xử lý
        Processing = 1,                // Đang xử lý
        Done = 2,                      // Đã xử lý
        Cancelled = 3,                 // Đã hủy
        Refund = 4,                    // Đã hoàn tiền
        DoneRefund = 5                 // Đã hoàn tiền xong
    }
}
