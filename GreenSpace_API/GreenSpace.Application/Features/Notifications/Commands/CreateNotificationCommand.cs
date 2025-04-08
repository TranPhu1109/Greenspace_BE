using FluentValidation;
using GreenSpace.Application.Repositories.MongoDbs;
using GreenSpace.Application.Services.Interfaces;
using GreenSpace.Application.ViewModels.MongoDbs.Notifications;
using GreenSpace.Domain.Entities.MongoDbs;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Notifications.Commands
{
    public class CreateNotificationCommand : IRequest<NotificationViewModel?>
    {
        public NotificationCreateModel Model = new();
        public class CommandValidation : AbstractValidator<CreateNotificationCommand>
        {
            public CommandValidation()
            {
                RuleFor(x => x.Model.Title)
                    .NotNull().NotEmpty();
                RuleFor(x => x.Model.Content)
                    .NotNull().NotEmpty();
                RuleFor(x => x.Model.Source)
                    .NotNull().NotEmpty();
            }
        }
        public class CommandHandler : IRequestHandler<CreateNotificationCommand, NotificationViewModel?>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly ILogger<CreateNotificationCommand> logger;
            private readonly IClaimsService claimsService;
            private readonly INotificationRepository notificationRepository;
            public CommandHandler(IUnitOfWork unitOfWork,
                IClaimsService claimsService, INotificationRepository notificationRepository,
                ILogger<CreateNotificationCommand> logger)
            {
                this.unitOfWork = unitOfWork;
                this.claimsService = claimsService;
                this.logger = logger;
                this.notificationRepository = notificationRepository;
            }
            public async Task<NotificationViewModel?> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
            {
                Guid userId = claimsService.GetCurrentUser;
                const string toolService = nameof(CreateNotificationCommand);
                logger.LogInformation($"Source: {toolService}_ Request", request.Model);
                logger.LogInformation($"Source: {toolService}_ current User: {userId}");
                if (userId == Guid.Empty)
                {
                    throw new ArgumentException("UserId is null");
                }

                var entity = unitOfWork.Mapper.Map<NotificationEntity>(request.Model);
                entity.UserId = userId;
                var result = await notificationRepository.CreateAsync(entity,
                    cancellationToken: cancellationToken);
                logger.LogInformation($"Source: {toolService}, result", result);
                if (result is not null)
                {
                    return unitOfWork.Mapper.Map<NotificationViewModel>(result);
                }
                else
                {
                    throw new Exception("Create Failed");
                }

            }
        }
    }
}