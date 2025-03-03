using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.Users
{
    public class UserViewModel
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = default!;
        public string? Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = default!;
        public Guid? StoreId { get; set; }
        public string RoleName { get; set; } = default!;
        public string? StoreName { get; set; }

    }
}