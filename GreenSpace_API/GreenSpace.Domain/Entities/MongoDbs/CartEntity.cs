using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Domain.Entities.MongoDbs
{
    public class CartEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonRepresentation(BsonType.String)]
        public Guid UserId { get; set; }

        public List<CartItemEntity> Items { get; set; } = new List<CartItemEntity>();
        public bool IsCurrent { get; set; } = true;
    }

}
