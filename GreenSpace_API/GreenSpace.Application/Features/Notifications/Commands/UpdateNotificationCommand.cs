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
    public class UpdateNotificationCommand : IRequest<bool>
    {
        public NotificationUpdateModel Model = new();
        public class CommandHandler : IRequestHandler<UpdateNotificationCommand, bool>
        {
            private readonly ILogger<UpdateNotificationCommand> logger;
            private readonly IUnitOfWork unitOfWork;
            private readonly IClaimsService claimsService;
            private readonly INotificationRepository notificationRepository;
            public CommandHandler(IUnitOfWork unitOfWork, IClaimsService claimsService,
                INotificationRepository notificationRepository,
                ILogger<UpdateNotificationCommand> logger)
            {
                this.logger = logger;
                this.unitOfWork = unitOfWork;
                this.claimsService = claimsService;
                this.notificationRepository = notificationRepository;
            }
            public async Task<bool> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
            {
                Guid userId = claimsService.GetCurrentUser;
                const string toolService = nameof(UpdateNotificationCommand);
                logger.LogInformation($"{toolService}, userId", userId);
                var entity = unitOfWork.Mapper.Map<NotificationEntity>(request.Model);
                entity.UserId = userId;
                var result = await notificationRepository.UpdateAsync(entity: entity,
                    cancellationToken: cancellationToken);
                logger.LogInformation($"{toolService}_Result", result);
                return result is not null;
            }
        }
    }
}
