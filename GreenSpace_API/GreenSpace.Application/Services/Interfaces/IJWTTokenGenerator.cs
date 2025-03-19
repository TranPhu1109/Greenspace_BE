using GreenSpace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Services.Interfaces
{
    public interface IJWTTokenGenerator
    {
        string GenerateToken(User user, string role);
    }
}
