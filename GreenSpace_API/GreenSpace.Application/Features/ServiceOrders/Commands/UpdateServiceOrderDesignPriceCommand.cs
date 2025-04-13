using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.SignalR;
using GreenSpace.Application.ViewModels.ServiceOrder;
using GreenSpace.Domain.Entities;
using GreenSpace.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.ServiceOrders.Commands
{
    public class UpdateServiceOrderDesignPriceCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public ServiceOrderUpdateDesignPriceModel UpdateModel { get; set; } = default!;
        public class CommmandValidation : AbstractValidator<UpdateServiceOrderDesignPriceCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");

            }
        }

        public class CommandHandler : IRequestHandler<UpdateServiceOrderDesignPriceCommand, bool>
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

            public async Task<bool> Handle(UpdateServiceOrderDesignPriceCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Update DesignPrice serviceOrder:\n");

                var servicerOrder = await _unitOfWork.ServiceOrderRepository.GetByIdAsync(request.Id);
                if (servicerOrder is null) throw new NotFoundException($"servicerOrder with Id-{request.Id} does not exist!");
                _mapper.Map(request.UpdateModel, servicerOrder);

             
                servicerOrder.TotalCost = (servicerOrder.MaterialPrice?? 0) + (request.UpdateModel.DesignPrice ?? 0);

            
                _unitOfWork.ServiceOrderRepository.Update(servicerOrder);

                var result = await _unitOfWork.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("messageReceived", "UpdateOrderService", $"{request.Id}");
                return result;
            }

        }
    }
}
