using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.Address
{
    public class AddressViewModel
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid UserId { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;   
        public string UserAddress { get; set; } = string.Empty;
        
    }
}
