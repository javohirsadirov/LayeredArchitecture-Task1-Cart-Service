// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

using Asp.Versioning;
using LayeredArchitectureTask1CartService.Business.CartServices.Exceptions;
using LayeredArchitectureTask1CartService.Business.CartServices.Interfaces;
using LayeredArchitectureTask1CartService.Dtos.CartService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LayeredArchitectureTask1CartService.Controllers.V1;

/// <summary>
/// Cart operations (API version 1).
/// </summary>
[ApiController]
[Authorize]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CartController(ICartService cartService) : ControllerBase
{
    /// <summary>
    /// Gets cart information by key.
    /// </summary>
    /// <param name="key">Cart unique key.</param>
    /// <returns>Cart model with key and list of items.</returns>
    [HttpGet("{key}")]
    [Authorize(Roles = "Manager,Customer")]
    [ProducesResponseType(typeof(CartDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCart(string key)
    {
        var cart = await cartService.GetCartAsync(key);
        if (cart == null)
        {
            return this.NotFound();
        }

        return this.Ok(cart);
    }

    /// <summary>
    /// Adds an item to the cart. Creates the cart if it does not exist.
    /// </summary>
    /// <param name="key">Cart unique key.</param>
    /// <param name="item">Cart item to add.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpPost("{key}/items")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddItem(string key, [FromBody] ItemDto item)
    {
        try
        {
            await cartService.AddItemAsync(key, item);
            return this.Ok();
        }
        catch (ValidationException ex)
        {
            return this.BadRequest(new { error = ex.Message });
        }
    }

    /// <summary>
    /// Deletes an item from the cart.
    /// </summary>
    /// <param name="key">Cart unique key.</param>
    /// <param name="itemId">Item identifier.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [HttpDelete("{key}/items/{itemId}")]
    [Authorize(Roles = "Manager")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteItem(string key, int itemId)
    {
        var removed = await cartService.RemoveItemAsync(key, itemId);
        if (!removed)
        {
            return this.NotFound();
        }

        return this.Ok();
    }
}
