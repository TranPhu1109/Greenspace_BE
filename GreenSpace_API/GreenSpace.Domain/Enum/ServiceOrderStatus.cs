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
        DeterminingDesignPrice = 2,    // Đang xác định giá 
        DepositSuccessful = 3,         // Đặt cọc thành công
        AssignToDesigner = 4,          // Đã giao cho nhà thiết kế
        DeterminingMaterialPrice = 5,   // xác dịnh giá vật liệu
        DoneDesign = 6,                // Hoàn thành thiết kế
        PaymentSuccess = 7,            // Thanh toán thành công
        Processing =8,                // Đang xử lý
        PickedPackageAndDelivery = 9,  // Đã lấy hàng & đang giao
        DeliveryFail = 10,              // Giao hàng thất bại
        ReDelivery = 11,                // Giao lại
        DeliveredSuccessfully = 12,     // Đã giao hàng thành công
        CompleteOrder  = 13,             // Hoàn thành đơn hàng
        OrderCancelled  = 14,           // Đơn hàng đã bị hủy
        Warning = 15,                 // cảnh báo vượt 30%
        Refund =16,
        DoneRefund = 17,
        StopService = 18,
        ReConsultingAndSketching = 19,     //  phác thảo lại
        ReDesign = 20,                       // thiết kế lại
        WaitDeposit = 21,                     // chờ đặt cọc
        DoneDeterminingDesignPrice = 22,
        DoneDeterminingMaterialPrice = 23,
    }
}
