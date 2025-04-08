using GreenSpace.Domain.Entities.MongoDbs;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Application.Repositories.MongoDbs
{
    public interface INotificationRepository
    {
        public Task<List<NotificationEntity>?> GetAll(CancellationToken cancellationToken = default);
        public Task<List<NotificationEntity>?> CreateManyAsync(List<NotificationEntity>? entities,
            CancellationToken cancellationToken = default);
        public Task<List<NotificationEntity>?> GetByUser(Guid userId,
            CancellationToken cancellationToken = default);
        public Task<bool> DeleteAllAsync(Guid userId,
            CancellationToken cancellationToken = default);
        public Task<bool> DeleteAsync(ObjectId id, CancellationToken cancellationToken = default);
        public Task<NotificationEntity?> CreateAsync(NotificationEntity entity,
            CancellationToken cancellationToken = default);
        Task<NotificationEntity?> UpdateAsync(NotificationEntity entity,
            CancellationToken cancellationToken = default);

    }
}
