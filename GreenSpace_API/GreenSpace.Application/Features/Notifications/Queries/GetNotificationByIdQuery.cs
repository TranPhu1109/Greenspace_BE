using GreenSpace.Application.Repositories.MongoDbs;
using GreenSpace.Application.ViewModels.MongoDbs.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Features.Notifications.Queries
{
    public class GetNotificationByIdQuery : IRequest<NotificationViewModel?>
    {
        // Todo
        public ObjectId Id;
        public class QueryHandler : IRequestHandler<GetNotificationByIdQuery, NotificationViewModel?>
        {
            private readonly IUnitOfWork unitOfWork;
            private readonly INotificationRepository notificationRepository;
            private ILogger<GetNotificationByIdQuery> logger;
            public QueryHandler(IUnitOfWork unitOfWork,
                ILogger<GetNotificationByIdQuery> logger,
                INotificationRepository notificationRepository)
            {
                this.unitOfWork = unitOfWork;
                this.logger = logger;
                this.notificationRepository = notificationRepository;
            }
            public async Task<NotificationViewModel?> Handle(GetNotificationByIdQuery request, CancellationToken cancellationToken)
            {
                const string toolService = nameof(GetNotificationByIdQuery);
                logger.LogInformation($"{toolService}_input", request.Id);
                var result = (await notificationRepository.GetAll(cancellationToken: cancellationToken))?.FirstOrDefault(x => x.Id == request.Id);
                logger.LogInformation($"{toolService}_result", result);
                return unitOfWork.Mapper.Map<NotificationViewModel>(result);
            }
        }
    }
}
