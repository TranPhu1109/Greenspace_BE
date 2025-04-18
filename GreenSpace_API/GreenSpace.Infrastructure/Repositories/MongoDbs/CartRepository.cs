﻿using GreenSpace.Application.Repositories.MongoDbs;
using GreenSpace.Application.ViewModels.MongoDbs.Carts;
using GreenSpace.Application;
using GreenSpace.Domain.Entities.MongoDbs;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenSpace.Infrastructure.Repositories.MongoDbs
{
    public class CartRepository : ICartRepository
    {
        private readonly IMongoCollection<CartEntity> cartCollection;
        private readonly IUnitOfWork unitOfWork;
        public CartRepository(IMongoDatabase db, IUnitOfWork unitOfWork)
        {
            cartCollection = db.GetCollection<CartEntity>("carts");
            this.unitOfWork = unitOfWork;
        }


        public async Task<bool> DeleteCartASync(Guid userId)
        {
            var cart = await cartCollection.FindOneAndUpdateAsync(x => x.UserId == userId && x.IsCurrent,
                Builders<CartEntity>.Update.Set(x => x.IsCurrent, false));
            return cart is not null;
        }

        public async Task<bool> RemoveProductFromCartAsync(Guid userId, Guid productId)
        {
            var cart = await cartCollection.Find(x => x.UserId == userId && x.IsCurrent).FirstOrDefaultAsync();
            var cartItem = cart?.Items.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem != null)
            {
                cart.Items.Remove(cartItem);
                await cartCollection.ReplaceOneAsync(x => x.UserId == userId && x.IsCurrent, cart);
                return true;
            }
            return false;
        }

        public async Task<CartViewModel?> GetCartByUserIdAsync(Guid userId)
        {
            var cart = (await cartCollection.FindAsync(x => x.UserId == userId && x.IsCurrent)).FirstOrDefault();
            return unitOfWork.Mapper.Map<CartViewModel>(cart);
        }

        public async Task<CartViewModel?> UpdateCartAsync(CartEntity entity)
        {
            var result = await cartCollection.UpdateOneAsync(
                Builders<CartEntity>.Filter.
                    Eq(x => x.Id, entity.Id),
                Builders<CartEntity>.Update
                    .Set(x => x.Items, entity.Items),
                    //.Set(x => x.No, entity.Note),
                    options: new UpdateOptions { IsUpsert = true }
            );
            return await GetCartByUserIdAsync(entity.UserId);

        }

        async Task<CartViewModel?> ICartRepository.CreateCartAsync(CartEntity model)
        {
            await cartCollection.InsertOneAsync(model);
            return await GetCartByUserIdAsync(model.UserId) ?? new();
        }


        public async Task<CartViewModel?> RemoveItemsAsync(Guid userId, List<Guid> productIds)
        {
            var cart = await cartCollection.Find(c => c.UserId == userId).FirstOrDefaultAsync();
            if (cart == null || cart.Items == null || !cart.Items.Any())
            {
                return null; 
            }
            cart.Items.RemoveAll(item => productIds.Contains(item.ProductId));

            await cartCollection.ReplaceOneAsync(c => c.UserId == userId, cart);
            return await GetCartByUserIdAsync(userId);
        }
    }
}
