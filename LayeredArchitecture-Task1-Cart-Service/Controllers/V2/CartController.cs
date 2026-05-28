using Asp.Versioning;
using LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Exceptions;
using LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Interfaces;
using LayeredArchitecture_Task1_Cart_Service.Dtos.CartService;
using Microsoft.AspNetCore.Mvc;

namespace LayeredArchitecture_Task1_Cart_Service.Controllers.V2;

/// <summary>
/// Cart operations (API version 2).
/// </summary>
[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CartController(ICartService cartService) : ControllerBase
{
    /// <summary>
    /// Gets cart items by cart key. Includes HATEOAS links.
    /// </summary>
    /// <param name="key">Cart unique key.</param>
    /// <returns>List of cart items with links.</returns>
    [HttpGet("{key}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCart(string key)
    {
        var cart = await cartService.GetCartAsync(key);
        if (cart == null)
            return NotFound();

        var links = new List<LinkDto>
        {
            new() { Href = Url.Action(nameof(GetCart), new { key })!, Rel = "self", Method = "GET" },
            new() { Href = Url.Action(nameof(AddItem), new { key })!, Rel = "add-item", Method = "POST" }
        };

        foreach (var item in cart.Items)
        {
            links.Add(new LinkDto
            {
                Href = Url.Action(nameof(DeleteItem), new { key, itemId = item.Id })!,
                Rel = "delete-item",
                Method = "DELETE"
            });
        }

        return Ok(new { Items = cart.Items, Links = links });
    }

    /// <summary>
    /// Adds an item to the cart. Creates the cart if it does not exist.
    /// </summary>
    /// <param name="key">Cart unique key.</param>
    /// <param name="item">Cart item to add.</param>
    [HttpPost("{key}/items")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddItem(string key, [FromBody] ItemDto item)
    {
        try
        {
            await cartService.AddItemAsync(key, item);
            return Ok();
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Deletes an item from the cart.
    /// </summary>
    /// <param name="key">Cart unique key.</param>
    /// <param name="itemId">Item identifier.</param>
    [HttpDelete("{key}/items/{itemId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteItem(string key, int itemId)
    {
        var removed = await cartService.RemoveItemAsync(key, itemId);
        if (!removed)
            return NotFound();

        return Ok();
    }
}
