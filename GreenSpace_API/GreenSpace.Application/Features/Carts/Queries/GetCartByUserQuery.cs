using GreenSpace.Application.Repositories.MongoDbs;
using GreenSpace.Application.Services.Interfaces;
using GreenSpace.Application.ViewModels.MongoDbs.Carts;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Carts.Queries
{
    public class GetCartByUserQuery : IRequest<CartViewModel?>
    {
        public class QueryHandler : IRequestHandler<GetCartByUserQuery, CartViewModel?>
        {
            private readonly IClaimsService claimsService;
            private readonly ICartRepository cartRepository;
            private readonly IUnitOfWork unitOfWork;
            public QueryHandler(IClaimsService claimsService,
                ICartRepository cartRepository,
                IUnitOfWork unitOfWork)
            {
                this.unitOfWork = unitOfWork;
                this.claimsService = claimsService;
                this.cartRepository = cartRepository;
            }
            public async Task<CartViewModel?> Handle(GetCartByUserQuery request, CancellationToken cancellationToken)
            {
                var currentUser = claimsService.GetCurrentUser;
                var result = await cartRepository.GetCartByUserIdAsync(currentUser);
                return result;
            }
        }
    }
    
}
