using GreenSpace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<Product>> Search(string? cate, string? name, float? minPrice, float? maxPrice);
    }
}
