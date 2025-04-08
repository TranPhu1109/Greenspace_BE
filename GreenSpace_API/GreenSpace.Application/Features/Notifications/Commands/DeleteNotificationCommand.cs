using GreenSpace.Application.Repositories.MongoDbs;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Notifications.Commands
{
    public class DeleteNotificationCommand : IRequest<bool>
    {
        public ObjectId Id { get; set; } = ObjectId.Empty;
        public class CommandHandler : IRequestHandler<DeleteNotificationCommand, bool>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly INotificationRepository notificationRepository;
            private readonly ILogger<DeleteNotificationCommand> logger;
            public CommandHandler(IUnitOfWork unitOfWork,
                ILogger<DeleteNotificationCommand> logger,
                INotificationRepository notificationRepository)
            {
                this.unitOfWork = unitOfWork;
                this.notificationRepository = notificationRepository;
                this.logger = logger;
            }
            public async Task<bool> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
            {
                const string toolService = nameof(DeleteNotificationCommand);
                logger.LogInformation($"{toolService}_input", request.Id);
                var deleteResult = await notificationRepository.DeleteAsync(request.Id,
                    cancellationToken: cancellationToken);
                return deleteResult;
            }
        }
    }
}
