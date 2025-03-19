using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.ViewModels.Users;

public class LoginResponseModel
{
    public UserViewModel User { get; set; } = default!;
    public string Token { get; set; } = default!;
}