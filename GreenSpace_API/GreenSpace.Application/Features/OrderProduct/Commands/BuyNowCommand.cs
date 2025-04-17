using GreenSpace.Application.Repositories;
using GreenSpace.Application.Services.Interfaces;
using GreenSpace.Application.ViewModels.OrderProducts;
using GreenSpace.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.OrderProduct.Commands
{
    public class BuyNowCommand : IRequest<OrderProductViewModel>
    {
        public CreateOrderModel Model { get; set; } = new();

        public class Handler : IRequestHandler<BuyNowCommand, OrderProductViewModel>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IOrderRepository orderRepository;
            private readonly IOrderDetailRepository orderDetailRepository;
            private readonly IProductRepository productRepository;
            private readonly IClaimsService claimsService;

            public Handler(IUnitOfWork unitOfWork, IOrderRepository orderRepository, IProductRepository productRepository, IClaimsService claimsService, IOrderDetailRepository orderDetailRepository)
            {
                this.unitOfWork = unitOfWork;
                this.orderRepository = orderRepository;
                this.productRepository = productRepository;
                this.claimsService = claimsService;
                this.orderDetailRepository = orderDetailRepository;
            }

            public async Task<OrderProductViewModel> Handle(BuyNowCommand request, CancellationToken cancellationToken)
            {
                var currentUser = claimsService.GetCurrentUser;
                var model = request.Model;

                // 1. Validate product
                var product = await productRepository.FirstOrDefaultAsync(x => x.Id == model.ProductId);
                if (product == null)
                    throw new Exception("Product not found");

                if (model.Quantity <= 0)
                    throw new Exception("Quantity must be greater than zero");

                // 2. Tính tổng tiền
                var productPrice = product.Price;
                var totalPrice = productPrice * model.Quantity + model.ShipPrice;

                // 3. Tạo đơn hàng
                var order = new Domain.Entities.Order
                {
                    Id = Guid.NewGuid(),
                    UserId = model.UserId,
                    UserName = model.UserName,
                    Address = model.Address,
                    Phone = model.Phone,
                    ShipPrice = model.ShipPrice,
                    TotalAmount = totalPrice,
                    CreationDate = DateTime.UtcNow,
                    Status = 0, // Pending
                };
                await orderRepository.AddAsync(order);
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = model.ProductId,
                    Quantity = model.Quantity,
                    Price = productPrice,
                };

                await orderDetailRepository.AddAsync(orderDetail);
                await unitOfWork.SaveChangesAsync();

                return unitOfWork.Mapper.Map<OrderProductViewModel>(order);
            }
        }
    }
    
}
