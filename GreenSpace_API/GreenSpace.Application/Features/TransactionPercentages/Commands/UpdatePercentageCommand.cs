using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.SignalR;
using GreenSpace.Application.ViewModels.ServiceOrder;
using GreenSpace.Application.ViewModels.TransactionPercentage;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.TransactionPercentages.Commands
{
    public class UpdatePercentageCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public TransactionPercentageCreateModel UpdateModel { get; set; } = default!;
        public class CommandValidation : AbstractValidator<UpdatePercentageCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.UpdateModel.DepositPercentage)
                      .GreaterThanOrEqualTo(30.0m)
                      .WithMessage("Deposit must be at least 30%")
                      .LessThanOrEqualTo(80.0m)
                      .WithMessage("Deposit must not exceed 80%");

                RuleFor(x => x.UpdateModel.RefundPercentage)
                    .GreaterThanOrEqualTo(10.0m)
                    .WithMessage("Refund must be at least 10%")
                    .LessThanOrEqualTo(50.0m)
                    .WithMessage("Refund must not exceed 50%");
            }
        }

        public class CommandHandler : IRequestHandler<UpdatePercentageCommand, bool>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private ILogger<CommandHandler> _logger;
            private AppSettings _appSettings;
            private readonly IHubContext<SignalrHub> _hubContext;
            private readonly IMediator _mediator;
            public CommandHandler(IUnitOfWork unitOfWork,
                    ILogger<CommandHandler> logger,
                    IMapper mapper,
                    AppSettings appSettings,
                   IHubContext<SignalrHub> hubContext,
                   IMediator mediator)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _mapper = mapper;
                _appSettings = appSettings;
                _hubContext = hubContext;
                _mediator = mediator;
            }

            public async Task<bool> Handle(UpdatePercentageCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Update ContructionPrice  serviceOrder:\n");

                var percentage = await _unitOfWork.TransactionPercentageRepository.GetByIdAsync(request.Id);
                if (percentage is null) throw new NotFoundException($"TransactionPercentage with Id-{request.Id} does not exist!");
                _mapper.Map(request.UpdateModel, percentage);



                _unitOfWork.TransactionPercentageRepository.Update(percentage);

                var result = await _unitOfWork.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("messageReceived", "Update", $"{request.Id}");
                return result;
            }

        }
    }
}
