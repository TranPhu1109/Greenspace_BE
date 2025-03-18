﻿using GreenSpace.Domain.Enum;

namespace GreenSpace.Application.ViewModels.Users
{
    public class LoginRequestModel
    {
        public string Token { get; set; } = string.Empty;
        public string? FCMToken { get; set; }
        public string? Role { get; set; } = nameof(RoleEnum.Customer);
    }
}
