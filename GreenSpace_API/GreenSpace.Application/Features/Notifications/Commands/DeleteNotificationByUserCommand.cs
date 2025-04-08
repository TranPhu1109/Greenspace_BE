using GreenSpace.Application.Repositories.MongoDbs;
using GreenSpace.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Notifications.Commands
{
    public class DeleteNotificationByUserCommand : IRequest<bool>
    {

        public class CommandHandler : IRequestHandler<DeleteNotificationByUserCommand, bool>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly IClaimsService claimsService;
            private readonly INotificationRepository notificationRepository;
            private readonly ILogger<DeleteNotificationByUserCommand> logger;
            public CommandHandler(IUnitOfWork unitOfWork,
                ILogger<DeleteNotificationByUserCommand> logger,
                INotificationRepository notificationRepository,
                IClaimsService claimsService)
            {
                this.claimsService = claimsService;
                this.notificationRepository = notificationRepository;
                this.logger = logger;
                this.unitOfWork = unitOfWork;
            }
            public async Task<bool> Handle(DeleteNotificationByUserCommand request, CancellationToken cancellationToken)
            {
                Guid userId = claimsService.GetCurrentUser;
                const string toolService = nameof(DeleteNotificationByUserCommand);
                logger.LogInformation($"{toolService}, currentUser", userId);
                var result = await notificationRepository.DeleteAllAsync(userId: userId,
                    cancellationToken: cancellationToken);
                return result;
            }
        }
    }
}
