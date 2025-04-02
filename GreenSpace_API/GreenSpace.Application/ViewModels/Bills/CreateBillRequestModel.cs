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
        public Guid? OrderId { get; set; } = default!;
        public Guid? ServiceOrderId { get; set; } = default!;
        public decimal Amount { get; set; } 
        public string Description { get; set; } = string.Empty;
    }
}
