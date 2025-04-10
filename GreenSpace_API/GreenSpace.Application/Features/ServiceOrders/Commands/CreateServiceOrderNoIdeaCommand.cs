using AutoMapper;
using FluentValidation;
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
    public class CreateServiceOrderNoIdeaCommand : IRequest<ServiceOrderViewModel>
    {
        public ServiceOrderNoUsingCreateModel CreateModel { get; set; } = default!;

        public class CommandValidation : AbstractValidator<CreateServiceOrderNoIdeaCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.CreateModel.UserId).NotNull().NotEmpty().WithMessage("userId  must not be null or empty");


                RuleFor(x => x.CreateModel.Address).NotNull().NotEmpty().WithMessage("Address must not be null or empty");

                RuleFor(x => x.CreateModel.CusPhone).NotNull().NotEmpty().WithMessage("CusPhone must not be null or empty").Matches(@"^\d+$").WithMessage("CusPhone must be a valid phone number");


            }
        }
        public class CommandHandler : IRequestHandler<CreateServiceOrderNoIdeaCommand, ServiceOrderViewModel>
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


            public async Task<ServiceOrderViewModel> Handle(CreateServiceOrderNoIdeaCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Creating a new order ");

                // Tạo mới Image
                var image = _mapper.Map<Image>(request.CreateModel.Image);
                image.Id = Guid.NewGuid();
                await _unitOfWork.ImageRepository.AddAsync(image);

                // Tạo mới Product
                var serviceOrder = _mapper.Map<ServiceOrder>(request.CreateModel);
                serviceOrder.Id = Guid.NewGuid();
                serviceOrder.ImageId = image.Id;
                serviceOrder.ServiceType = ServiceTypeEnum.NoDesignIdea.ToString();
                serviceOrder.Status = (int)ServiceOrderStatus.Pending;
                serviceOrder.IsCustom = true;
                foreach (var item in request.CreateModel.ServiceOrderDetails)
                {
                    var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                    if (product == null)
                    {
                        throw new ApplicationException($"Product with ID {item.ProductId} not found");
                    }

                    var detail = new ServiceOrderDetail
                    {
                        Id = Guid.NewGuid(),
                        ServiceOrderId = serviceOrder.Id,
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        Price = product.Price,
                        TotalPrice = product.Price * item.Quantity
                    };


                    serviceOrder.ServiceOrderDetails.Add(detail);
                }
                serviceOrder.MaterialPrice = serviceOrder.ServiceOrderDetails.Sum(d => d.TotalPrice);

                await _unitOfWork.ServiceOrderRepository.AddAsync(serviceOrder);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<ServiceOrderViewModel>(serviceOrder);
            }


        }
    }
}
