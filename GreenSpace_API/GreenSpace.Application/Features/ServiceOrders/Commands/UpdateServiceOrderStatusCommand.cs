using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.ViewModels.Category;
using GreenSpace.Application.ViewModels.ServiceOrder;
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
    public class UpdateServiceOrderStatusCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public ServiceOrderUpdateStatusModel UpdateModel { get; set; } = default!;
        public class CommmandValidation : AbstractValidator<UpdateServiceOrderStatusCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
              
            }
        }

        public class CommandHandler : IRequestHandler<UpdateServiceOrderStatusCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<CommandHandler> _logger;
            private AppSettings _appSettings;
            public CommandHandler(IUnitOfWork unitOfWork,
                    ILogger<CommandHandler> logger,
                    IMapper mapper,
                    AppSettings appSettings)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _mapper = mapper;
                _appSettings = appSettings;
            }

            public async Task<bool> Handle(UpdateServiceOrderStatusCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Update status serviceOrder:\n");

                var servicerOrder = await _unitOfWork.ServiceOrderRepository.GetByIdAsync(request.Id ,x => x.ServiceOrderDetails);
                if (servicerOrder is null) throw new NotFoundException($"servicerOrder with Id-{request.Id} does not exist!");

                if (!Enum.IsDefined(typeof(ServiceOrderStatus), request.UpdateModel.Status))
                {
                    throw new InvalidOperationException($"Invalid status value: {request.UpdateModel.Status}");
                }
                if (request.UpdateModel.Status == 8)
                {
                    foreach (var detail in servicerOrder.ServiceOrderDetails)
                    {
                        var product = await _unitOfWork.ProductRepository.GetByIdAsync(detail.ProductId);
                        if (product == null)
                        {
                            throw new NotFoundException($"Product with Id-{detail.ProductId} not found!");
                        }

                        if (product.Stock < 0)
                        {
                            throw new InvalidOperationException($"Not enough stock for Product Id {product.Id}");
                        }
                        if (product.Stock < detail.Quantity)
                        {
                            throw new InvalidOperationException( $"Not enough stock for Product Id {product.Id}. Available: {product.Stock}, Required: {detail.Quantity}");
                        }

                        product.Stock -= detail.Quantity;
                        _unitOfWork.ProductRepository.Update(product);
                    }
                }
                _mapper.Map(request.UpdateModel, servicerOrder);
                _unitOfWork.ServiceOrderRepository.Update(servicerOrder);

                var result = await _unitOfWork.SaveChangesAsync();
                return result;
            }

        }
    }
}
