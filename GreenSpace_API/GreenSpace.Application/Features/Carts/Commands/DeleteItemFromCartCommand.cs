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
    public class DeleteItemFromCartCommand : IRequest<CartViewModel>
    {
        public Guid ProductId { get; set; }

        public DeleteItemFromCartCommand(Guid productId)
        {
            ProductId = productId;
        }
        public class DeleteItemFromCartCommandHandler : IRequestHandler<DeleteItemFromCartCommand, CartViewModel?>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly ICartRepository cartRepository;
            private readonly IClaimsService claimsService;

            public DeleteItemFromCartCommandHandler(IUnitOfWork unitOfWork, ICartRepository cartRepository, IClaimsService claimsService)
            {
                this.unitOfWork = unitOfWork;
                this.cartRepository = cartRepository;
                this.claimsService = claimsService;
            }

            public async Task<CartViewModel?> Handle(DeleteItemFromCartCommand request, CancellationToken cancellationToken)
            {
                var currentUser = claimsService.GetCurrentUser;

                // Lấy cart hiện tại
                var existingCart = await cartRepository.GetCartByUserIdAsync(currentUser);
                if (existingCart == null)
                {
                    throw new Exception("Cart not found");
                }

                // Kiểm tra item trong cart
                var itemToRemove = existingCart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
                if (itemToRemove == null)
                {
                    throw new Exception($"ProductId {request.ProductId} not found in cart");
                }

                // Xóa sản phẩm
                existingCart.Items.Remove(itemToRemove);

                // Cập nhật lại cart
                var updatedCart = unitOfWork.Mapper.Map<CartEntity>(existingCart);
                await cartRepository.UpdateCartAsync(updatedCart);

                return await cartRepository.GetCartByUserIdAsync(currentUser);
            }
        }
    }
}
