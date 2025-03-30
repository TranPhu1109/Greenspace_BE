using GreenSpace.Application.ViewModels.MongoDbs.Carts;
using GreenSpace.Domain.Entities.MongoDbs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Repositories.MongoDbs
{
    public interface ICartRepository
    {
        public Task<CartViewModel?> CreateCartAsync(CartEntity model);
        public Task<CartViewModel?> GetCartByUserIdAsync(Guid userId);
        public Task<bool> DeleteCartASync(Guid userId);
        public Task<CartViewModel?> UpdateCartAsync(CartEntity entity);

    }
}
