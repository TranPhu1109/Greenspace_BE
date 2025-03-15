using GreenSpace.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Repositories
{
    public interface IProductDetailRepository : IGenericRepository<ProductDetail>
    {
        Task RemoveProductDetail(ProductDetail productDetail);

    }
}
