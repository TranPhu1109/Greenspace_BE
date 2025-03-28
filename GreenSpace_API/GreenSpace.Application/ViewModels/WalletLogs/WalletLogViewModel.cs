using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.WalletLogs
{
    public class WalletLogViewModel
    {
        public string Source { get; set; } = string.Empty;
        public string TxnRef { get; set; } = string.Empty;
        public string TransactionNo { get; set; } = string.Empty;

        [Precision(18, 2)]
        public decimal Amount { get; set; } = default!;
        public string? Type { get; set; }
        public Guid WalletId { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

    }
}
