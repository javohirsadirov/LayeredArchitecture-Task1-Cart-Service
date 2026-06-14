// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

using LayeredArchitectureTask1CartService.Business.CartServices.Exceptions;
using LayeredArchitectureTask1CartService.Business.CartServices.Interfaces;
using LayeredArchitectureTask1CartService.Dtos.CartService;
using LayeredArchitectureTask1CartService.Repository.CartService.Interfaces;
using LayeredArchitectureTask1CartService.Repository.Models;

namespace LayeredArchitectureTask1CartService.Business.CartServices.Implementation;

/// <summary>
/// Business logic service for cart operations.
/// </summary>
internal class CartService(ICartRepository cartRepository) : ICartService
{
    /// <inheritdoc/>
    public async Task<CartDto?> GetCartAsync(string cartKey)
    {
        var cart = await cartRepository.GetCartAsync(cartKey);
        if (cart == null)
        {
            return null;
        }

        return new CartDto
        {
            Key = cart.Key,
            Items = [.. cart.Items.Select(item => new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                Quantity = item.Quantity,
                ImageAltText = item.ImageAltText,
                ImageUrl = item.ImageUrl,
            })],
        };
    }

    /// <inheritdoc/>
    public async Task AddItemAsync(string cartKey, ItemDto item)
    {
        if (item.Price < 0)
        {
            throw new ValidationException("Price cannot be negative.");
        }

        if (item.Quantity <= 0)
        {
            throw new ValidationException("Quantity must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(item.Name))
        {
            throw new ValidationException("Name is required.");
        }

        await cartRepository.AddItemAsync(cartKey, new Item
        {
            Id = item.Id,
            Name = item.Name,
            Price = item.Price,
            CartKey = cartKey,
            Quantity = item.Quantity,
            ImageAltText = item.ImageAltText,
            ImageUrl = item.ImageUrl,
        });
    }

    /// <inheritdoc/>
    public async Task<bool> RemoveItemAsync(string cartKey, int itemId)
    {
        return await cartRepository.RemoveItemAsync(cartKey, itemId);
    }

    /// <inheritdoc/>
    public async Task UpdateItemsByProductIdAsync(int productId, string name, decimal price, string? imageUrl, string? imageAltText)
    {
        await cartRepository.UpdateItemsByProductIdAsync(productId, name, price, imageUrl, imageAltText);
    }
}
