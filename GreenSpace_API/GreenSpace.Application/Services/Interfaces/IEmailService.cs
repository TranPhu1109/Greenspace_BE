﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Services.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string email, string subject, string message);
}
