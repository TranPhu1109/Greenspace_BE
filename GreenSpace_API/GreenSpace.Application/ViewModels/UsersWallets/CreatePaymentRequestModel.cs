using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.UsersWallets
{
    public class CreatePaymentRequestModel
    {
        public Guid WalletId { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? ServiceOrderId { get; set; }
        public double Amount { get; set; } // số tiền cần thanh toán
        public string Description { get; set; } = string.Empty;
    }
}
