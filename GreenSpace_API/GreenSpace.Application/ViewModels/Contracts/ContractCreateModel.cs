using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.Contracts
{
    public class ContractCreateModel
    {
        public Guid UserId { get; set; }
        public Guid ServiceOrderId { get; set; }
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = default!;

        public decimal? DesignPrice { get; set; } = default!;

    }
}
