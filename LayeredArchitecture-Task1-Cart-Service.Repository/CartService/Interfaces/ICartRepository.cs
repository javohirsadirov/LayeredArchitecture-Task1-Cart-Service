using LayeredArchitecture_Task1_Cart_Service.Repository.Models;

namespace LayeredArchitecture_Task1_Cart_Service.Repository.CartService.Interfaces;

public interface ICartRepository
{
    Task<Cart?> GetCartAsync(string cartKey);

    Task AddItemAsync(string cartKey, Item item);

    Task<bool> RemoveItemAsync(string cartKey, int itemId);

    Task UpdateItemsByProductIdAsync(int productId, string name, decimal price, string? imageUrl, string? imageAltText);
}