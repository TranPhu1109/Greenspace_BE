using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Domain.Enum
{
    public enum ComplaintStatusEnum
    {
        pending = 0,                  //đang chờ
        ItemArrivedAtWarehouse = 1,      // hàng đã về kho kiểm tra
        Approved = 2,                //Đã phê duyệt
        Processing = 3,              //Đang xử lý
        refund = 4,                  //Hoàn tiền 
        Complete = 5,
        reject = 6,                    // từ chối
        Delivery = 7,                //Giao hàng 
        delivered = 8,                   // đã giao hàng lại
       

    }
}
