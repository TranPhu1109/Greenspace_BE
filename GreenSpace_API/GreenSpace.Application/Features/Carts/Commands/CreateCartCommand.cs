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
    public class CreateCartCommand : IRequest<CartViewModel?>
    {
        public CartCreateModel model { get; set; } = new();
        public class CommandHandler : IRequestHandler<CreateCartCommand, CartViewModel?>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly ICartRepository cartRepository;
            private readonly IClaimsService claimsService;
            public CommandHandler(IUnitOfWork unitOfWork, ICartRepository cartRepository,
                IClaimsService claimsService)
            {
                this.unitOfWork = unitOfWork;
                this.cartRepository = cartRepository;
                this.claimsService = claimsService;
            }
            public async Task<CartViewModel?> Handle(CreateCartCommand request, CancellationToken cancellationToken)
            {
                var currentUser = claimsService.GetCurrentUser;
                var cart = unitOfWork.Mapper.Map<CartEntity>(request.model);

                cart.UserId = currentUser;
                if (cart.Items.Any())
                {
                    foreach (var cartItem in cart.Items)
                    {
                        if (await unitOfWork.ProductRepository.FirstOrDefaultAsync(x => x.Id == cartItem.ProductId) is null)
                        {
                            throw new Exception($"ProductId not exsit {cartItem.ProductId}");
                        }
                    }
                }

                return await cartRepository.CreateCartAsync(cart);

            }
        }
    }
}
