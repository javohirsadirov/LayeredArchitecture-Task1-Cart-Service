// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

using LayeredArchitectureTask1CartService.Dtos.CartService;

namespace LayeredArchitectureTask1CartService.Business.CartServices.Interfaces;

/// <summary>
/// Defines operations for the cart business service.
/// </summary>
public interface ICartService
{
    /// <summary>Gets a cart by key.</summary>
    /// <param name="cartKey">The cart key.</param>
    /// <returns>The cart DTO, or null if not found.</returns>
    Task<CartDto?> GetCartAsync(string cartKey);

    /// <summary>Adds an item to the cart.</summary>
    /// <param name="cartKey">The cart key.</param>
    /// <param name="item">The item to add.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task AddItemAsync(string cartKey, ItemDto item);

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
