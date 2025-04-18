﻿using AutoMapper;
using FluentValidation;
using GreenSpace.Application.Repositories.MongoDbs;
using GreenSpace.Application.ViewModels.OrderProducts;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;

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
                var cart = await _cartRepository.GetCartByUserIdAsync(request.CreateModel.UserId);

                if (cart == null || !cart.Items.Any())
                {
                    throw new ApplicationException("Cart is empty or not found");
                }

                var selectedItems = request.CreateModel.Products;
                if (selectedItems == null || !selectedItems.Any())
                {
                    throw new ApplicationException("Không có sản phẩm nào được chọn để thanh toán.");
                }
                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    UserId = request.CreateModel.UserId,
                    UserName = request.CreateModel.UserName,
                    Address = request.CreateModel.Address,
                    Phone = request.CreateModel.Phone,
                    ShipPrice = request.CreateModel.ShipPrice,
                    Status = 0, // Pending
                    OrderDate = DateTime.UtcNow,
                   
                };

                decimal totalAmount = 0;
                List<Product> orderedProducts = new List<Product>();

                foreach (var cartItem in selectedItems)
                {
                    var product = await _unitOfWork.ProductRepository.GetByIdAsync(cartItem.ProductId);
                    if (product == null)
                    {
                        throw new ApplicationException($"Sản phẩm với ID {cartItem.ProductId} không tồn tại");
                    }

                    if (product.Stock < cartItem.Quantity)
                    {
                        throw new ApplicationException($"Chỉ còn {product.Stock} sản phẩm {product.Name} trong kho");
                    }

                    var orderDetail = new OrderDetail
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        Price = product.Price
                    };
                    await _unitOfWork.OrderDetailRepository.AddAsync(orderDetail);
                    order.OrderDetails.Add(orderDetail);
                    totalAmount += product.Price * cartItem.Quantity;
                    product.Stock -= cartItem.Quantity;
                    _unitOfWork.ProductRepository.Update(product);
                    orderedProducts.Add(product);

                }

                await _cartRepository.RemoveItemsAsync(request.CreateModel.UserId, selectedItems.Select(x => x.ProductId).ToList());

                order.TotalAmount = totalAmount + request.CreateModel.ShipPrice;

                await _unitOfWork.OrderRepository.AddAsync(order);
                try
                {
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    var innerExceptionMessage = ex.InnerException?.Message ?? ex.Message;
                    throw new Exception($"Lỗi khi lưu dữ liệu: {innerExceptionMessage}", ex);
                }

                var orderViewModel = _mapper.Map<OrderProductViewModel>(order);
                //orderViewModel.Products = orderedProducts;

                return orderViewModel;
            }
        }
    }
}
