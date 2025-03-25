using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.Bills
{
    public class BillViewModel
    {
        [Precision(18, 10)]
        public decimal Amount { get; set; } = default!;
        public Guid WalletId { get; set; }
        public Guid? OrderId { get; set; }
        public Guid? ServiceOrderId { get; set; }
        public string Description { get; set; } = default!;
    }
}
