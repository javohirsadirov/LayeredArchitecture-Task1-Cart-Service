using LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Interfaces;
using LayeredArchitecture_Task1_Cart_Service.Dtos.CartService;
using LayeredArchitecture_Task1_Cart_Service.Repository.CartService.Interfaces;
using LayeredArchitecture_Task1_Cart_Service.Repository.Models;

namespace LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Implementation;

internal class CartService(ICartRepository cartRepository) : ICartService
{
    public async Task AddToCartAsync(Guid cartId, ItemDto item)
    {
        await cartRepository.AddToCartAsync(cartId, new Item
        {
            Id = item.Id,
            Name = item.Name,
            Price = item.Price,
            CartId = item.Id,
            Quantity = item.Quantity,
            ImageAltText = item.ImageAltText,
            ImageUrl = item.ImageUrl
        });
    }

    public async Task<CartDto> GetCartListAsync(Guid cartId)
    {
        return await cartRepository.GetCartListAsync(cartId) switch
        {
            null => new CartDto() { Id = cartId },
            var cart => new CartDto
            {
                Id = cart.Id,
                CartItems = cart.CartItems.Select(item => new ItemDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ImageAltText = item.ImageAltText,
                    ImageUrl = item.ImageUrl
                }).ToList()
            }
        };
    }

    public async Task RemoveFromCartAsync(Guid cartId, Guid itemId)
    {
        await cartRepository.RemoveFromCartAsync(cartId, itemId);
    }
}