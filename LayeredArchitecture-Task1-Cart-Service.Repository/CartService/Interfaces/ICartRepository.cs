// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

using LayeredArchitectureTask1CartService.Repository.Models;

namespace LayeredArchitectureTask1CartService.Repository.CartService.Interfaces;

/// <summary>
/// Defines operations for the cart repository.
/// </summary>
public interface ICartRepository
{
    /// <summary>Gets a cart by its key.</summary>
    /// <param name="cartKey">The cart key.</param>
    /// <returns>The cart, or null if not found.</returns>
    Task<Cart?> GetCartAsync(string cartKey);

    /// <summary>Adds an item to the cart.</summary>
    /// <param name="cartKey">The cart key.</param>
    /// <param name="item">The item to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddItemAsync(string cartKey, Item item);

    /// <summary>Removes an item from the cart.</summary>
    /// <param name="cartKey">The cart key.</param>
    /// <param name="itemId">The item identifier.</param>
    /// <returns>True if the item was removed; otherwise, false.</returns>
    Task<bool> RemoveItemAsync(string cartKey, int itemId);

    /// <summary>Updates items by product identifier.</summary>
    /// <param name="productId">The product identifier.</param>
    /// <param name="name">The new name.</param>
    /// <param name="price">The new price.</param>
    /// <param name="imageUrl">The new image URL.</param>
    /// <param name="imageAltText">The new image alt text.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateItemsByProductIdAsync(int productId, string name, decimal price, string? imageUrl, string? imageAltText);
}
