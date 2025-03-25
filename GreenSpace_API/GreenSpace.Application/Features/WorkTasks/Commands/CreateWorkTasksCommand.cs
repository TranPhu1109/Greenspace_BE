using AutoMapper;
using FluentValidation;
using GreenSpace.Application.ViewModels.Category;
using GreenSpace.Application.ViewModels.WorkTasks;
using GreenSpace.Domain.Entities;
using GreenSpace.Domain.Enum;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.WorkTasks.Commands
{
    public class CreateWorkTasksCommand : IRequest<WorkTaskViewModel>
    {
        public WorkTaskCreateModel CreateModel { get; set; } = default!;
        public class CommmandValidation : AbstractValidator<CreateWorkTasksCommand>
        {
            public CommmandValidation()
            {
                RuleFor(x => x.CreateModel.ServiceOrderId).NotNull().NotEmpty().WithMessage("ServiceOrderId must not null or empty");
                RuleFor(x => x.CreateModel.UserId).NotNull().NotEmpty().WithMessage("UserID must not null or empty");
            }
        }
        public class CommandHandler : IRequestHandler<CreateWorkTasksCommand, WorkTaskViewModel>
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
            public async Task<WorkTaskViewModel> Handle(CreateWorkTasksCommand request, CancellationToken cancellationToken)
            {
                _logger.LogInformation("Create Task:\n");
                var task = _mapper.Map<WorkTask>(request.CreateModel);
                task.Id = Guid.NewGuid();
                task.Status = (int)WorkTasksEnum.Pending;
                await _unitOfWork.WorkTaskRepository.AddAsync(task);
                await _unitOfWork.SaveChangesAsync();
                return _mapper.Map<WorkTaskViewModel>(task);
            }
        }
    }
}
