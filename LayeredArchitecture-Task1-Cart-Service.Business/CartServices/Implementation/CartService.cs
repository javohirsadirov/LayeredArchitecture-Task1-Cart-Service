using LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Exceptions;
using LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Interfaces;
using LayeredArchitecture_Task1_Cart_Service.Dtos.CartService;
using LayeredArchitecture_Task1_Cart_Service.Repository.CartService.Interfaces;
using LayeredArchitecture_Task1_Cart_Service.Repository.Models;

namespace LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Implementation;

internal class CartService(ICartRepository cartRepository) : ICartService
{
    public async Task<CartDto?> GetCartAsync(string cartKey)
    {
        var cart = await cartRepository.GetCartAsync(cartKey);
        if (cart == null)
            return null;

        return new CartDto
        {
            Key = cart.Key,
            Items = cart.Items.Select(item => new ItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Price = item.Price,
                Quantity = item.Quantity,
                ImageAltText = item.ImageAltText,
                ImageUrl = item.ImageUrl
            }).ToList()
        };
    }

    public async Task AddItemAsync(string cartKey, ItemDto item)
    {
        if (item.Price < 0)
            throw new ValidationException("Price cannot be negative.");

        if (item.Quantity <= 0)
            throw new ValidationException("Quantity must be greater than zero.");

        if (string.IsNullOrWhiteSpace(item.Name))
            throw new ValidationException("Name is required.");

        await cartRepository.AddItemAsync(cartKey, new Item
        {
            Id = item.Id,
            Name = item.Name,
            Price = item.Price,
            CartKey = cartKey,
            Quantity = item.Quantity,
            ImageAltText = item.ImageAltText,
            ImageUrl = item.ImageUrl
        });
    }

    public async Task<bool> RemoveItemAsync(string cartKey, int itemId)
    {
        return await cartRepository.RemoveItemAsync(cartKey, itemId);
    }
}