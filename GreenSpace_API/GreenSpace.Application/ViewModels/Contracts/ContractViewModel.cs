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
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public Guid? ModificatedBy { get; set; } = default!;
        public DateTime? ModificationDate { get; set; } = null;
    }
}
