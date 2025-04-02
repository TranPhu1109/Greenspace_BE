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

                // 1. Lấy cart hiện tại của user nếu có
                var existingCart = await cartRepository.GetCartByUserIdAsync(currentUser);

                // 2. Nếu cart đã tồn tại ➜ thêm sản phẩm mới vào
                if (existingCart != null)
                {
                    foreach (var newItem in request.model.Items ?? new List<CartItemCreateModel>())
                    {
                        if (await unitOfWork.ProductRepository.FirstOrDefaultAsync(x => x.Id == newItem.ProductId) is null)
                        {
                            throw new Exception($"ProductId not exist {newItem.ProductId}");
                        }

                        var existingItem = existingCart.Items.FirstOrDefault(i => i.ProductId == newItem.ProductId);
                        if (existingItem != null)
                        {
                            // Cộng dồn số lượng nếu sản phẩm đã có
                            existingItem.Quantity += newItem.Quantity;
                        }
                        else
                        {
                            // Thêm sản phẩm mới
                            existingCart.Items.Add(new CartItemViewModel
                            {
                                ProductId = newItem.ProductId,
                                Quantity = newItem.Quantity
                            });
                        }
                    }

                    var updatedCartEntity = unitOfWork.Mapper.Map<CartEntity>(existingCart);
                    await cartRepository.UpdateCartAsync(updatedCartEntity);
                    return await cartRepository.GetCartByUserIdAsync(currentUser);
                }

                // 3. Nếu chưa có cart ➜ tạo mới như cũ
                var cart = unitOfWork.Mapper.Map<CartEntity>(request.model);
                cart.UserId = currentUser;

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
