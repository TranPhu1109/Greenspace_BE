using AutoMapper;
using FluentValidation;
using GreenSpace.Application.Repositories.MongoDbs;
using GreenSpace.Application.ViewModels.OrderProducts;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.OrderProduct.Commands
{
    public class CreateOrderFromCartCommand : IRequest<OrderProductViewModel>
    {
        public CreateOrderProductModel CreateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<CreateOrderFromCartCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.CreateModel.UserId).NotEmpty().WithMessage("UserId must not be empty");
            }
        }

        public class CommandHandler : IRequestHandler<CreateOrderFromCartCommand, OrderProductViewModel>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly ILogger<CommandHandler> _logger;
            private readonly AppSettings _appSettings;
            private readonly ICartRepository _cartRepository;

            public CommandHandler(
                IUnitOfWork unitOfWork,
                IMapper mapper,
                ILogger<CommandHandler> logger,
                AppSettings appSettings, 
                ICartRepository cartRepository)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;
                _appSettings = appSettings;
                _cartRepository = cartRepository;
            }

            public async Task<OrderProductViewModel> Handle(CreateOrderFromCartCommand request, CancellationToken cancellationToken)
            {
                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    UserId = request.CreateModel.UserId,
                    Address = request.CreateModel.Address,
                    Phone = request.CreateModel.Phone,
                    ShipPrice = request.CreateModel.ShipPrice,
                    Status = 1, // Pending
                    OrderDate = DateTime.UtcNow,
                    OrderDetails = new List<OrderDetail>()
                };

                decimal totalAmount = 0;
                List<Product> orderedProducts = new List<Product>();

                var cart = await _cartRepository.GetCartByUserIdAsync(request.CreateModel.UserId);

                if (cart == null || !cart.Items.Any())
                {
                    throw new ApplicationException("Cart is empty or not found");
                }

                foreach (var cartItem in cart.Items)
                {
                    var product = await _unitOfWork.ProductRepository.GetByIdAsync(cartItem.ProductId);

                    if (product == null)
                    {
                        throw new ApplicationException($"Product with ID {cartItem.ProductId} not found");
                    }

                    if (product.Stock < cartItem.Quantity)
                    {
                        throw new ApplicationException($"Only {product.Stock} units of {product.Name} are available");
                    }

                    var orderDetail = new OrderDetail
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Price
                    };
                    await _unitOfWork.OrderDetailRepository.AddAsync(orderDetail);
                    order.OrderDetails.Add(orderDetail);
                    totalAmount += cartItem.Price * cartItem.Quantity;
                    product.Stock -= cartItem.Quantity;
                    _unitOfWork.ProductRepository.Update(product);
                    orderedProducts.Add(product);
                }

                await _cartRepository.DeleteCartASync(request.CreateModel.UserId);

                order.TotalAmount = totalAmount + request.CreateModel.ShipPrice;

                await _unitOfWork.OrderRepository.AddAsync(order);
                await _unitOfWork.SaveChangesAsync();

                var orderViewModel = _mapper.Map<OrderProductViewModel>(order);
                orderViewModel.Products = orderedProducts;

                return orderViewModel;
            }
        }
    }
}
