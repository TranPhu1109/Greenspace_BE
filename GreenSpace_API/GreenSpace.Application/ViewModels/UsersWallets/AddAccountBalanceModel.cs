using GreenSpace.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.UsersWallets;

public class AddAccountBalanceModel
{
    public decimal Amount { get; set; } = 0;
    public string TxnRef { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Type { get; set; } = WalletLogTypeEnum.Deposit.ToString();

}
