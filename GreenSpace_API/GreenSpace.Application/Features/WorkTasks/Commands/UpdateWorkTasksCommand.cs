using AutoMapper;
using FluentValidation;
using GreenSpace.Application.GlobalExceptionHandling.Exceptions;
using GreenSpace.Application.SignalR;
using GreenSpace.Application.ViewModels.Category;
using GreenSpace.Application.ViewModels.WorkTasks;
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
    public class UpdateWorkTasksCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public WorkTaskUpdateModel UpdateModel { get; set; } = default!;
        public class CommmandValidation : AbstractValidator<UpdateWorkTasksCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not null or empty");
                RuleFor(x => x.UpdateModel.ServiceOrderId).NotNull().NotEmpty().WithMessage("ServiceOrderId must not null or empty");
                RuleFor(x => x.UpdateModel.UserId).NotNull().NotEmpty().WithMessage("UserID must not null or empty");
            }
        }

        public class CommandHandler : IRequestHandler<UpdateWorkTasksCommand, bool>
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

            public async Task<bool> Handle(UpdateWorkTasksCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Update task:\n");
                var task = await _unitOfWork.WorkTaskRepository.GetByIdAsync(request.Id);
                if (task is null) throw new NotFoundException($"Task with Id-{request.Id} is not exist!");
                _mapper.Map(request.UpdateModel, task);
                _unitOfWork.WorkTaskRepository.Update(task);
                var result = await _unitOfWork.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("messageReceived", "UpdateTask", $"{request.Id} - {(WorkTasksEnum)request.UpdateModel.Status}");
                return result;
            }
        }
    }
}
