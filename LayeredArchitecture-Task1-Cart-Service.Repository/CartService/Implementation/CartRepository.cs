using LayeredArchitecture_Task1_Cart_Service.Repository.CartService.Interfaces;
using LayeredArchitecture_Task1_Cart_Service.Repository.Models;
using LiteDB;

namespace LayeredArchitecture_Task1_Cart_Service.Repository.CartService.Implementation;

internal class CartRepository(LiteDatabase _database) : ICartRepository
{
    public async Task AddToCartAsync(Guid cartId, Item cartItem)
    {
        await Task.Run(() =>
        {
            _database.GetCollection<Item>("Items").Upsert(cartItem);
        });
    }

    public async Task<Cart> GetCartListAsync(Guid cartId)
    {
        return await Task.Run(() =>
        {
            var items = _database.GetCollection<Item>("Items").FindAll().ToList();

            return new Cart { 
                Id = cartId,
                CartItems = items
            };
        });
    }

    public async Task RemoveFromCartAsync(Guid cartId, Guid itemId)
    {
        await Task.Run(() =>
        {
            _database.GetCollection<Item>("Items").Delete(itemId);
        });
    }
}