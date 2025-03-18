using GreenSpace.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.Users
{
    public class RegisterRequestModel
    {
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc")]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string? Role { get; set; } = nameof(RoleEnum.Customer);
        
    }
}
