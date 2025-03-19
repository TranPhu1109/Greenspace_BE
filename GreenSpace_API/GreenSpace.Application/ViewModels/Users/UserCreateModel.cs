using GreenSpace.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.Users
{
    public class UserCreateModel
    {
        public string Name { get; set; } = default!;
        public string? Email { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
        //public string? FCMToken { get; set; } = string.Empty;
        public string? Phone { get; set; } = default!;
        public string? Address { get; set; } = string.Empty;
        public string? RoleName { get; set; } = string.Empty;
    }
}
