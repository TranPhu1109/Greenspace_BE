using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.ServiceOrder
{
    public class ServiceOrderUpdateDepositModel
    {
        public decimal DepositPercentage { get; set; } = decimal.One;
        public decimal RefundPercentage { get; set; } = decimal.One;
    }
}
