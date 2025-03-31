using GreenSpace.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Domain.Entities
{
    public class WalletLog : BaseEntity
    {
        public string Source { get; set; } = string.Empty;

        [Precision(18, 2)]
        public decimal Amount { get; set; } = default!;
        public string Status { get; set; } = nameof(WalletLogStatusEnum.Sucess);
        public string Type { get; set; } = nameof(WalletLogTypeEnum.Deposit);
        public string? TxnRef { get; set; } = string.Empty;
        public string TransactionNo { get; set; } = string.Empty;

        #region  Relationship Config
        public Guid WalletId { get; set; }
        public UsersWallet UsersWallet { get; set; } = default!;
        #endregion
    }
}
