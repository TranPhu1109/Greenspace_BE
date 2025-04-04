﻿using GreenSpace.Application.Repositories;
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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        protected readonly AppDbContext _context;
        public ProductRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }
        public async Task<List<Product>> Search(string? cate, string? name, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Materials
                .Include(p => p.Category)
                .Include(p => p.Image)
                .AsQueryable();

            if (!string.IsNullOrEmpty(cate))
                query = query.Where(p => p.Category.Name.Contains(cate));

            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.Name.Contains(name));

            if (minPrice.HasValue)
                query = query.Where(p => p.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(p => p.Price <= maxPrice.Value);

            return await query.ToListAsync(); 
        }

    }

}
