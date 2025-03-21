using GreenSpace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Repositories
{
    public interface  IServiceOrderRepository : IGenericRepository<ServiceOrder>
    {
        Task<List<ServiceOrder>> SearchNoUsingIdea(string? phone, string? username, int? status);
        Task<List<ServiceOrder>> SearchUsingIdea(string? phone, string? username, int? status);
    }
}
