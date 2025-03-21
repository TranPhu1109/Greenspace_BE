using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Domain.Enum
{
    public enum ServiceOrderStatus
    {
        Pending = 0,                   // Chờ xử lý
        ConsultingAndSketching = 1,    // Đang tư vấn & phác thảo
        DepositSuccessful = 2,         // Đặt cọc thành công
        DeterminingMaterialPrice = 3,  // Đang xác định giá vật liệu
        AssignToDesigner = 4,          // Đã giao cho nhà thiết kế
        DoneDesign = 5,                // Hoàn thành thiết kế
        PaymentSuccess = 6,            // Thanh toán thành công
        Processing =7,                // Đang xử lý
        PickedPackageAndDelivery = 8,  // Đã lấy hàng & đang giao
        DeliveryFail = 9,              // Giao hàng thất bại
        ReDelivery = 10,                // Giao lại
        DeliveredSuccessfully = 11,     // Đã giao hàng thành công
        CompleteOrder  = 12,             // Hoàn thành đơn hàng
        OrderCancelled  = 13,           // Đơn hàng đã bị hủy
        Warning = 14
    }
}
