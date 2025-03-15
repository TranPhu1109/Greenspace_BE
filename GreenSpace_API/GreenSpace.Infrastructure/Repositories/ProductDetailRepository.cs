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
    public class ProductDetailRepository : GenericRepository<ProductDetail>, IProductDetailRepository
    {
        protected readonly AppDbContext _context;
        public ProductDetailRepository(AppDbContext context, ICurrentTime timeService, IClaimsService claimsService) : base(context, timeService, claimsService)
        {
            _context = context;
        }
        public async Task RemoveProductDetail(ProductDetail productDetail)
        {
            var existingProductDetail = await _context.ProductDetails
          .FirstOrDefaultAsync(pd => pd.Id == productDetail.Id);

            if (existingProductDetail != null)
            {
                _context.ProductDetails.Remove(existingProductDetail);
                await _context.SaveChangesAsync();
            }
        }


    }
}
