using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public class UsersWallet : BaseEntity
{
    public int WalletId { get; set; }
    [Precision(18, 2)]
    public decimal Amount { get; set; }
    public string WalletAccount { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
