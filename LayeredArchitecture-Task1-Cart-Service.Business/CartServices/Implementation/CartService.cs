using LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Interfaces;
using LayeredArchitecture_Task1_Cart_Service.Dtos.CartService;

namespace LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Implementation;

internal class CartService : ICartService
{
    public async Task AddToCartAsync(Guid cartId, ItemDto cartItem)
    {
        throw new NotImplementedException();
    }

    public async Task<List<CartDto>> GetCartListAsync(Guid cartId)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveFromCartAsync(Guid cartId, Guid itemId)
    {
        throw new NotImplementedException();
    }
}