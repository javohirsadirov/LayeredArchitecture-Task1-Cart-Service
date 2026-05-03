namespace LayeredArchitecture_Task1_Cart_Service.Dtos.CartService;

/// <summary>
/// Represents a cart with its items.
/// </summary>
public class CartDto
{
    /// <summary>Cart unique key.</summary>
    public required string Key { get; set; }

    /// <summary>List of items in the cart.</summary>
    public List<ItemDto> Items { get; set; } = [];
}