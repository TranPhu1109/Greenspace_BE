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
    public class DesignIdeaRepository : GenericRepository<DesignIdea>, IDesignIdeaRepository
    {
        protected readonly AppDbContext _context;
        public DesignIdeaRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }
        public async Task<List<DesignIdea>> Search(string? cate, string? name, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.DesignIdeas
                .Include(p => p.DesignIdeasCategory)
                .Include(p => p.Image)
                .Include(p => p.ProductDetails)
                .AsQueryable();

            if (!string.IsNullOrEmpty(cate))
                query = query.Where(p => p.DesignIdeasCategory.Name.Contains(cate));

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));

            if (minPrice.HasValue)
                query = query.Where(p => p.TotalPrice >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.TotalPrice <= maxPrice.Value);

            return await query.ToListAsync();
        }


    }
}
