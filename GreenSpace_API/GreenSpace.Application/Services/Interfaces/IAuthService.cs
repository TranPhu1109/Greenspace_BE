using GreenSpace.Application.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseModel> LoginAsync(string token, string? FCMToken, string role);
        Task<LoginResponseModel> RefreshTokenAsync(string token);
    }
}
