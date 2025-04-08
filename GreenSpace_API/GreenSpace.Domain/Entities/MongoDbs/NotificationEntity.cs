using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Domain.Entities.MongoDbs
{
    public enum NotificationSourceEnum
    {
        Admin
        
    }
    public class NotificationEntity
    {
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public string Title { get; set; } = string.Empty;
        public bool IsSeen { get; set; } = false;
        public string Content { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public NotificationSourceEnum Source { get; set; } = NotificationSourceEnum.Admin;
        public DateTime CreateDate { get; set; }
        public Guid UserId { get; set; } = Guid.Empty;
    }
}
