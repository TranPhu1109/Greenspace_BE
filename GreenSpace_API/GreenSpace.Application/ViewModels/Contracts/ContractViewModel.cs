using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.Contracts
{
    public class ContractViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ServiceOrderId { get; set; }
        public string? UserName { get; set; }
        public string Description { get; set; } = default!;

        public Guid? ModificatedBy { get; set; } = default!;
        public DateTime? ModificationDate { get; set; } = null;
    }
}
