using AutoMapper;
using FluentValidation;
using GreenSpace.Application.ViewModels.Category;
using GreenSpace.Application.ViewModels.Products;
using GreenSpace.Application.ViewModels.ServiceOrder;
using GreenSpace.Domain.Entities;
using GreenSpace.Domain.Enum;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ServiceOrders.Commands
{
    public class CreateServiceOrderCommand : IRequest<ServiceOrderViewModel>
    {
        public ServiceOrderCreateModel CreateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<CreateServiceOrderCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.CreateModel.UserId).NotNull().NotEmpty().WithMessage("userId  must not be null or empty");

                RuleFor(x => x.CreateModel.DesignIdeaId).NotNull().WithMessage("DesignIdeaId must not be empty");

                RuleFor(x => x.CreateModel.Address).NotNull().NotEmpty().WithMessage("Address must not be null or empty");

                RuleFor(x => x.CreateModel.CusPhone).NotNull().NotEmpty().WithMessage("CusPhone must not be null or empty").Matches(@"^\d+$").WithMessage("CusPhone must be a valid phone number");

            }
        }
        public class CommandHandler : IRequestHandler<CreateServiceOrderCommand, ServiceOrderViewModel>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<CommandHandler> _logger;
            private AppSettings _appSettings;

            public CommandHandler(IUnitOfWork unitOfWork,
                    IMapper mapper,
                    ILogger<CommandHandler> logger,
                    AppSettings appSettings)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _logger = logger;
                _appSettings = appSettings;
            }

            public async Task<ServiceOrderViewModel> Handle(CreateServiceOrderCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Create OrderService:\n");


                var design = request.CreateModel.DesignIdeaId.HasValue? 
                    await _unitOfWork.DesignIdeaRepository.GetByIdAsync(request.CreateModel.DesignIdeaId.Value, x => x.ProductDetails): null;
                var serviceOrder = _mapper.Map<ServiceOrder>(request.CreateModel);
                serviceOrder.Id = Guid.NewGuid();
                serviceOrder.ServiceType = ServiceTypeEnum.UsingDesignIdea.ToString();

                serviceOrder.Status = (int)ServiceOrderStatus.Pending;

                if (request.CreateModel.IsCustom == true)
                {
                    // tao ảnh
                    var image = _mapper.Map<Image>(request.CreateModel.Image);
                    image.Id = Guid.NewGuid();
                    await _unitOfWork.ImageRepository.AddAsync(image);
                    serviceOrder.ImageId = image.Id;
                }
                foreach (var item in request.CreateModel.Products)
                {
                    var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                    if (product == null)
                    {
                        throw new ApplicationException($"Product with ID {item.ProductId} not found");
                    }
                    else
                    {
                        var serviceOrderDetail = new ServiceOrderDetail
                        {
                            Id = Guid.NewGuid(),
                            ServiceOrderId = serviceOrder.Id,
                            ProductId = product.Id,
                            Quantity = item.Quantity,
                            Price = product.Price,
                            TotalPrice = product.Price * item.Quantity,
                        };
                        await _unitOfWork.ServiceOrderDetailRepository.AddAsync(serviceOrderDetail);
                        serviceOrder.MaterialPrice += serviceOrderDetail.TotalPrice;
                        
                    }
                    product.Stock -= item.Quantity;   
                    
                }
                // add danh sach san pham cua designidea vao serviceOrderDetails
                //if (design != null && design.ProductDetails.Any())
                //    {
                //        var serviceOrderDetails = design.ProductDetails.Select(pd => new ServiceOrderDetail
                //        {
                //            Id = Guid.NewGuid(), 
                //            ProductId = pd.ProductId,
                //            ServiceOrderId = serviceOrder.Id, 
                //            Quantity = pd.Quantity, 
                //            Price = pd.Price / pd.Quantity,
                //            TotalPrice = pd.Price,
                //        }).ToList();

                //        await _unitOfWork.ServiceOrderDetailRepository.AddRangeAsync(serviceOrderDetails);
                //    }

                //    serviceOrder.MaterialPrice = design?.ProductDetails.Sum(pd => pd.Price) ?? 0;
                

                await _unitOfWork.ServiceOrderRepository.AddAsync(serviceOrder);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<ServiceOrderViewModel>(serviceOrder);
            }



        }
    }
}
