using GreenSpace.Application.Repositories;
using GreenSpace.Application.Services.Interfaces;
using GreenSpace.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Infrastructure.Repositories
{
    public class ServiceOrderDetailRepository : GenericRepository<ServiceOrderDetail>, IServiceOrderDetailRepository
    {
        protected readonly AppDbContext _context;
        public ServiceOrderDetailRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }
        public async Task RemoveServiceOrderDetail(ServiceOrderDetail productDetail)
        {
            var existingDetail = await _context.ServiceOrderDetails
          .FirstOrDefaultAsync(pd => pd.Id == productDetail.Id);

            if (existingDetail != null)
            {
                _context.ServiceOrderDetails.Remove(existingDetail);
                await _context.SaveChangesAsync();
            }
        }
    }
}
