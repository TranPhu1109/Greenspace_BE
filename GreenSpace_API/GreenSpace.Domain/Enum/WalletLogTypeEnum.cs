using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Domain.Enum
{
    public enum WalletLogTypeEnum
    {
        Withdraw,
        Deposit
    }

    public enum WalletLogStatusEnum
    {
        Pending,
        Sucess,
        Failed
    }
}
