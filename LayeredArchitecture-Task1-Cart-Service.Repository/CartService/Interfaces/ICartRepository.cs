using LayeredArchitecture_Task1_Cart_Service.Repository.Models;

namespace LayeredArchitecture_Task1_Cart_Service.Repository.CartService.Interfaces;

public interface ICartRepository
{
    public Task<Cart> GetCartListAsync(Guid cartId);

    public Task AddToCartAsync(Guid cartId, Item cartItem);

    public Task RemoveFromCartAsync(Guid cartId, Guid itemId);
}