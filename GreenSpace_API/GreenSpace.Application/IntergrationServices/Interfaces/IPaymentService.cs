 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.IntergrationServices.Interfaces
{
    public interface IPaymentService
    {
        public const string VN_PAY_BASE_URL = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html?";
    }
}
