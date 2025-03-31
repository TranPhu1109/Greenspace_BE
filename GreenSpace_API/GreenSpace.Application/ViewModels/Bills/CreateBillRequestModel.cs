using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.Bills
{
    public class CreateBillRequestModel
    {
        public Guid WalletId { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? ServiceOrderId { get; set; }
        public double Amount { get; set; } 
        public string Description { get; set; } = string.Empty;
    }
}
