using GreenSpace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Repositories
{
    public interface IServiceOrderDetailRepository : IGenericRepository<ServiceOrderDetail>
    {
        Task RemoveServiceOrderDetail(ServiceOrderDetail productDetail);
    }
}
