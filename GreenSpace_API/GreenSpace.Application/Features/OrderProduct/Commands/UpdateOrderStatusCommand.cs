using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.SignalR;
using GreenSpace.Application.ViewModels.OrderProducts;
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

namespace GreenSpace.Application.Features.OrderProduct.Commands
{
    public class UpdateOrderStatusCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public OrderUpdateStatusModel UpdateModel { get; set; } = default!;
        public class CommmandValidation : AbstractValidator<UpdateOrderStatusCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");

            }
        }

        public class CommandHandler : IRequestHandler<UpdateOrderStatusCommand, bool>
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

            public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Update status Order:\n");

                var order = await _unitOfWork.OrderRepository.GetByIdAsync(request.Id);
                if (order is null) throw new NotFoundException($"servicerOrder with Id-{request.Id} does not exist!");

                if (!Enum.IsDefined(typeof(OrderProductStatus), request.UpdateModel.Status))
                {
                    throw new InvalidOperationException($"Invalid status value: {request.UpdateModel.Status}");
                }
                _mapper.Map(request.UpdateModel, order);
                _unitOfWork.OrderRepository.Update(order);

                var result = await _unitOfWork.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("messageReceived", "CreateOrderProduct", $"{request.Id}");
                return result;
            }

        }
    }
}
