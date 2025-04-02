using GreenSpace.Application.Repositories.MongoDbs;
using GreenSpace.Application.Services.Interfaces;
using GreenSpace.Application.ViewModels.MongoDbs.Carts;
using GreenSpace.Domain.Entities.MongoDbs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Carts.Commands
{
    public class UpdateCartCommand : IRequest<CartViewModel?>
    {
        public CartUpdateModel model { get; set; } = new();
        public class CommandHandler : IRequestHandler<UpdateCartCommand, CartViewModel?>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IClaimsService claimsService;
            private readonly ICartRepository cartRepository;
            public CommandHandler(IUnitOfWork unitOfWork,
                IClaimsService claimsService,
                ICartRepository cartRepository)
            {
                this.unitOfWork = unitOfWork;
                this.claimsService = claimsService;
                this.cartRepository = cartRepository;
            }
            public async Task<CartViewModel?> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
            {
                var currentUser = claimsService.GetCurrentUser;
                var cart = unitOfWork.Mapper.Map<CartEntity>(request.model);
                cart.UserId = currentUser;
                if (cart.Items.Any())
                {
                    foreach (var item in cart.Items)
                    {
                        if (await unitOfWork.ProductRepository.FirstOrDefaultAsync(x => x.Id == item.ProductId) is null)
                        {
                            throw new Exception($"Sản phẩm không tồn tại");
                        }
                    }
                }
                var res = await cartRepository.UpdateCartAsync(cart);
                return await cartRepository.GetCartByUserIdAsync(currentUser);
            }
        }
    }
}
