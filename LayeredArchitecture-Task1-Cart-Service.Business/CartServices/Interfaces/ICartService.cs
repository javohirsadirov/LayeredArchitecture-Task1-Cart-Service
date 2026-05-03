using LayeredArchitecture_Task1_Cart_Service.Dtos.CartService;

namespace LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Interfaces;

public interface ICartService
{
    Task<CartDto?> GetCartAsync(string cartKey);

    Task AddItemAsync(string cartKey, ItemDto item);

    Task RemoveItemAsync(string cartKey, int itemId);
}