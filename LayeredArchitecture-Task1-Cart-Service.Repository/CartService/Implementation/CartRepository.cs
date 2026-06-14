// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

using LayeredArchitectureTask1CartService.Repository.CartService.Interfaces;
using LayeredArchitectureTask1CartService.Repository.Models;
using LiteDB;

namespace LayeredArchitectureTask1CartService.Repository.CartService.Implementation;

/// <summary>
/// Repository implementation for cart operations using LiteDB.
/// </summary>
internal sealed class CartRepository(LiteDatabase database) : ICartRepository
{
    private ILiteCollection<Item> Items => database.GetCollection<Item>("Items");

    /// <inheritdoc/>
    public Task<Cart?> GetCartAsync(string cartKey)
    {
        var items = this.Items.Find(i => i.CartKey == cartKey).ToList();
        if (items.Count == 0)
        {
            return Task.FromResult<Cart?>(null);
        }

        return Task.FromResult<Cart?>(new Cart { Key = cartKey, Items = items });
    }

    /// <inheritdoc/>
    public Task AddItemAsync(string cartKey, Item item)
    {
        item.CartKey = cartKey;
        this.Items.Upsert(item);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task<bool> RemoveItemAsync(string cartKey, int itemId)
    {
        var item = this.Items.FindOne(i => i.CartKey == cartKey && i.Id == itemId);
        if (item is null)
        {
            return Task.FromResult(false);
        }

        this.Items.Delete(item.Id);
        return Task.FromResult(true);
    }

    /// <inheritdoc/>
    public Task UpdateItemsByProductIdAsync(int productId, string name, decimal price, string? imageUrl, string? imageAltText)
    {
        var items = this.Items.Find(i => i.Id == productId).ToList();
        foreach (var item in items)
        {
            item.Name = name;
            item.Price = price;
            item.ImageUrl = imageUrl;
            item.ImageAltText = imageAltText;
            this.Items.Update(item);
        }

        return Task.CompletedTask;
    }
}
