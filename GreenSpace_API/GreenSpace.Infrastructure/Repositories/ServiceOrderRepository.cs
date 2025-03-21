using GreenSpace.Application.Repositories;
using GreenSpace.Application.Services.Interfaces;
using GreenSpace.Domain.Entities;
using GreenSpace.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Infrastructure.Repositories
{
    public class ServiceOrderRepository : GenericRepository<ServiceOrder>, IServiceOrderRepository
    {
        protected readonly AppDbContext _context;
        public ServiceOrderRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }
        public async Task<List<ServiceOrder>> SearchUsingIdea(string? phone, string? username, int? status)
        {
            var query = _context.ServiceOrders
                .Include(p => p.User)
                .Include(p => p.Image)
                .Include(p => p.ServiceOrderDetails)
                .AsQueryable()
                .Where(p => p.ServiceType == ServiceTypeEnum.UsingDesignIdea.ToString());

            if (!string.IsNullOrEmpty(username))
                query = query.Where(p => p.User.Name.Contains(username));

            if (!string.IsNullOrEmpty(phone))
                query = query.Where(p => p.CusPhone.Contains(phone));

            if (status.HasValue) 
                query = query.Where(p => p.Status == (int)(ServiceOrderStatus)status.Value);

            return await query.ToListAsync();
        }
        public async Task<List<ServiceOrder>> SearchNoUsingIdea(string? phone, string? username, int? status)
        {
            var query = _context.ServiceOrders
                .Include(p => p.User)
                .Include(p => p.Image)
                .Include(p => p.ServiceOrderDetails)
                .AsQueryable()
                .Where(p => p.ServiceType == ServiceTypeEnum.NoDesignIdea.ToString());

            if (!string.IsNullOrEmpty(username))
                query = query.Where(p => p.User.Name.Contains(username));

            if (!string.IsNullOrEmpty(phone))
                query = query.Where(p => p.CusPhone.Contains(phone));

            if (status.HasValue)
                query = query.Where(p => p.Status == (int)(ServiceOrderStatus)status.Value);

            return await query.ToListAsync();
        }
    }
}
