using GreenSpace.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public class UsersWallet : BaseEntity
{
    public string Name { get; set; } = default!;
    [Precision(18, 2)]
    public decimal Amount { get; set; }
    public string WalletType { get; set; } = nameof(WalletTypeEnum.Customer);
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Bill> Bills { get; set; } = new List<Bill>();
    public ICollection<WalletLog> WalletLogs { get; set; } = default!;
}
