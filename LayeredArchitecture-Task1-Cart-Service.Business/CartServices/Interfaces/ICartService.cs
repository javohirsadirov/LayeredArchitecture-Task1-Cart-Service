using LayeredArchitecture_Task1_Cart_Service.Dtos.CartService;

namespace LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Interfaces;

public interface ICartService
{
    Task<CartDto?> GetCartAsync(string cartKey);

    Task AddItemAsync(string cartKey, ItemDto item);

    Task<bool> RemoveItemAsync(string cartKey, int itemId);

    Task UpdateItemsByProductIdAsync(int productId, string name, decimal price, string? imageUrl, string? imageAltText);
}