using GreenSpace.Application.ViewModels.Bills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.UsersWallets;

public class WalletViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Amount { get; set; }
    public string? WalletType { get; set; }
    public Guid? UserId { get; set; }
    public IEnumerable<BillViewModel> Bills { get; set; } = new List<BillViewModel>();

}
