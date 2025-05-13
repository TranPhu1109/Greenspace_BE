using AutoMapper;
using FluentValidation;
using GreenSpace.Application.SignalR;
using GreenSpace.Application.ViewModels.WorkTasks;
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

namespace GreenSpace.Application.Features.WorkTasks.Commands
{
    public class CreateContructTaskCommand : IRequest<WorkTaskViewModel>
    {
        public WorkTaskCreateModel CreateModel { get; set; } = default!;
        public class CommmandValidation : AbstractValidator<CreateContructTaskCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.CreateModel.ServiceOrderId).NotNull().NotEmpty().WithMessage("ServiceOrderId must not null or empty");
                RuleFor(x => x.CreateModel.UserId).NotNull().NotEmpty().WithMessage("UserID must not null or empty");
            }
        }
        public class CommandHandler : IRequestHandler<CreateContructTaskCommand, WorkTaskViewModel>
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
            public async Task<WorkTaskViewModel> Handle(CreateContructTaskCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Create Task:\n");
                var task = _mapper.Map<WorkTask>(request.CreateModel);
                task.Id = Guid.NewGuid();
                task.Status = (int)WorkTasksEnum.Pending;
                await _unitOfWork.WorkTaskRepository.AddAsync(task);
                await _unitOfWork.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("messageReceived", "CreateTask", $"{task.Id}");
                return _mapper.Map<WorkTaskViewModel>(task);
            }
        }
    }
}
