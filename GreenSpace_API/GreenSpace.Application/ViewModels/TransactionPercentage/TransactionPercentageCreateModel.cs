using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.TransactionPercentage
{
    public class TransactionPercentageCreateModel
    {
        public decimal DepositPercentage { get; set; } = 100m;
        public decimal RefundPercentage { get; set; } = 100m;
    }
}
