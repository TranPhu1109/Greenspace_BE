using System;
using System.Collections.Generic;

namespace GreenSpace.Domain.Entities;

public partial class UsersWallet
{
    public Guid UserId { get; set; }

    public int? WalletId { get; set; }

    public string? WalletAccount { get; set; }

    public virtual User User { get; set; } = null!;
}
