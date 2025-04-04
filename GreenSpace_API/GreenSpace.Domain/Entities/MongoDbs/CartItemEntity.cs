﻿using Microsoft.EntityFrameworkCore.Storage.Json;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Domain.Entities.MongoDbs
{
    public class CartItemEntity
    {
        [BsonRepresentation(BsonType.String)]
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }   
        public string ProductImage { get; set; } = string.Empty;
    }
}
