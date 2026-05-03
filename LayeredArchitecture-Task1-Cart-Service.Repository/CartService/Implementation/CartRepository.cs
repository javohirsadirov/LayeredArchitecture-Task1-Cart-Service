using LayeredArchitecture_Task1_Cart_Service.Repository.CartService.Interfaces;
using LayeredArchitecture_Task1_Cart_Service.Repository.Models;
using LiteDB;

namespace LayeredArchitecture_Task1_Cart_Service.Repository.CartService.Implementation;

internal class CartRepository(LiteDatabase _database) : ICartRepository
{
    private ILiteCollection<Item> Items => _database.GetCollection<Item>("Items");

    public Task<Cart?> GetCartAsync(string cartKey)
    {
        var items = Items.Find(i => i.CartKey == cartKey).ToList();
        if (items.Count == 0)
            return Task.FromResult<Cart?>(null);

        return Task.FromResult<Cart?>(new Cart { Key = cartKey, Items = items });
    }

    public Task AddItemAsync(string cartKey, Item item)
    {
        item.CartKey = cartKey;
        Items.Upsert(item);
        return Task.CompletedTask;
    }

    public Task RemoveItemAsync(string cartKey, int itemId)
    {
        var item = Items.FindOne(i => i.CartKey == cartKey && i.Id == itemId);
        if (item is not null)
            Items.Delete(item.Id);

        return Task.CompletedTask;
    }
}