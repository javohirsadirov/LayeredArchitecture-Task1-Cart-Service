using LayeredArchitecture_Task1_Cart_Service.Dtos.CartService;

namespace LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Interfaces;

public interface ICartService
{
    public Task<CartDto> GetCartListAsync(Guid cartId);

    public Task AddToCartAsync(Guid cartId, ItemDto cartItem);

    public Task RemoveFromCartAsync(Guid cartId, Guid itemId);
}