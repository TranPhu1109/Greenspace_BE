using AutoMapper;
using FluentValidation;
using GreenSpace.Application.Features.Products.Commands;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.SignalR;
using GreenSpace.Application.ViewModels.DesignIdea;
using GreenSpace.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.DesignIdeas.Commands
{
    public class UpdateDesignIdeaCommand :IRequest<bool>
    {
        public Guid Id { get; set; }
        public DesignIdeaUpdateModel UpdateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<UpdateDesignIdeaCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.UpdateModel.Name).NotNull().NotEmpty().WithMessage("Name must not be null or empty");

                RuleFor(x => x.UpdateModel.DesignIdeasCategoryId).NotNull().WithMessage("CategoryId must not be empty");

                RuleFor(x => x.UpdateModel.DesignPrice).GreaterThan(0).WithMessage("Price must be greater than zero");

                RuleFor(x => x.UpdateModel.Description).NotNull().NotEmpty().WithMessage("Description must not be null or empty");

            }
        }
        public class CommandHandler : IRequestHandler<UpdateDesignIdeaCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<CommandHandler> _logger;
            private AppSettings _appSettings;
            private readonly IHubContext<SignalrHub> _hubContext;
            public CommandHandler(IUnitOfWork unitOfWork,
                    ILogger<CommandHandler> logger,
                    IMapper mapper,
                    AppSettings appSettings,
                   IHubContext<SignalrHub> hubContext)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _mapper = mapper;
                _appSettings = appSettings;
                _hubContext = hubContext;
            }

            public async Task<bool> Handle(UpdateDesignIdeaCommand request, CancellationToken cancellationToken)
            {
                var design = await _unitOfWork.DesignIdeaRepository.GetByIdAsync(request.Id, p => p.Image, p => p.ProductDetails);
                if (design is null) throw new NotFoundException($"DesignIdea with Id {request.Id} does not exist!");

                design.DesignImage1URL = !string.IsNullOrEmpty(request.UpdateModel.DesignImage1URL)
                                         ? request.UpdateModel.DesignImage1URL
                                         : design.DesignImage1URL;

                design.DesignImage2URL = !string.IsNullOrEmpty(request.UpdateModel.DesignImage2URL)
                                         ? request.UpdateModel.DesignImage2URL
                                         : design.DesignImage2URL;

                design.DesignImage3URL = !string.IsNullOrEmpty(request.UpdateModel.DesignImage3URL)
                                         ? request.UpdateModel.DesignImage3URL
                                         : design.DesignImage3URL;

                // Cập nhật ảnh
                if (request.UpdateModel.Image is not null)
                {
                    design.Image.ImageUrl = !string.IsNullOrEmpty(request.UpdateModel.Image.ImageUrl) ? request.UpdateModel.Image.ImageUrl : design.Image.ImageUrl;
                    design.Image.Image2 = !string.IsNullOrEmpty(request.UpdateModel.Image.Image2) ? request.UpdateModel.Image.Image2 : design.Image.Image2;
                    design.Image.Image3 = !string.IsNullOrEmpty(request.UpdateModel.Image.Image3) ? request.UpdateModel.Image.Image3 : design.Image.Image3;
                }


                // Nếu request có danh sách ProductDetails
                if (request.UpdateModel.ProductDetails != null && request.UpdateModel.ProductDetails.Any())
                {
                    var requestProducts = request.UpdateModel.ProductDetails;
                    var existProducts = design.ProductDetails.ToList();

                    // Thêm hoặc cập nhật ProductDetails
                    foreach (var productDetail in requestProducts)
                    {
                        var product = await _unitOfWork.ProductRepository.GetByIdAsync(productDetail.ProductId);
                        if (product == null)
                            throw new NotFoundException($"Product with Id {productDetail.ProductId} does not exist!");

                        var existingProductDetail = existProducts.FirstOrDefault(p => p.ProductId == productDetail.ProductId);

                        if (existingProductDetail != null)
                        {
                            // Nếu đã tồn tại, cập nhật
                            existingProductDetail.Quantity = productDetail.Quantity;
                            existingProductDetail.Price = product.Price * productDetail.Quantity;
                        }
                        else
                        {
                            // Nếu chưa tồn tại, thêm mới
                            var newProductDetail = new ProductDetail
                            {
                                Id = Guid.NewGuid(),
                                ProductId = productDetail.ProductId,
                                DesignIdeaId = design.Id,
                                Quantity = productDetail.Quantity,
                                Price = product.Price * productDetail.Quantity
                            };
                            await _unitOfWork.ProductDetailRepository.AddAsync(newProductDetail);
                            existProducts.Add(newProductDetail);
                        }
                    }


                    // đối chiếu so sánh vơi danh sach cũ nào ko cần nữa xóa 
                    var productIdsInRequest = requestProducts.Select(p => p.ProductId).ToHashSet();
                    var productsToRemove = existProducts.Where(p => !productIdsInRequest.Contains(p.ProductId)).ToList();

                    foreach (var p in productsToRemove)
                    {
                        design.ProductDetails.Remove(p);
                        await _unitOfWork.ProductDetailRepository.RemoveProductDetail(p);
                    }
                }
                design.MaterialPrice = design.ProductDetails.Sum(p => p.Price);
                design.TotalPrice = request.UpdateModel.DesignPrice + design.MaterialPrice;
                _mapper.Map(request.UpdateModel, design);
                _unitOfWork.DesignIdeaRepository.Update(design);

                var result = await _unitOfWork.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("messageReceived", "UpdateDesignIdea", $"{request.Id}");
                return result;


            }

        }
    }
}
