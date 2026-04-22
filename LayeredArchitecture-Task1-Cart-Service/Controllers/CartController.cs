using LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Interfaces;
using LayeredArchitecture_Task1_Cart_Service.Dtos.CartService;
using Microsoft.AspNetCore.Mvc;

namespace LayeredArchitecture_Task1_Cart_Service.Controllers;

[ApiController]
[Route("[controller]")]
public class CartController(ICartService cartService) : ControllerBase
{

    [HttpGet(Name = "GetCartList")]
    public async Task<IActionResult> GetCartList(Guid cartId)
    {
        var cartList = await cartService.GetCartListAsync(cartId);
        return Ok(cartList);
    }

    [HttpPost(Name = "AddToCart")]
    public async Task<IActionResult> AddToCart(Guid cartId, ItemDto cartItem)
    {
        await cartService.AddToCartAsync(cartId, cartItem);
        return Ok();
    }

    [HttpDelete(Name = "RemoveFromCart")]
    public async Task<IActionResult> RemoveFromCart(Guid cartId, Guid itemId)
    {
        await cartService.RemoveFromCartAsync(cartId, itemId);
        return Ok();
    }
}