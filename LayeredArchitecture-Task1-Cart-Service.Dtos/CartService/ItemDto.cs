namespace LayeredArchitecture_Task1_Cart_Service.Dtos.CartService;

/// <summary>
/// Represents a cart item.
/// </summary>
public class ItemDto
{
    /// <summary>Item unique identifier.</summary>
    public required int Id { get; set; }

    /// <summary>Item name.</summary>
    public required string Name { get; set; } = string.Empty;

    /// <summary>Image URL.</summary>
    public string? ImageUrl { get; set; }

    /// <summary>Image alt text.</summary>
    public string? ImageAltText { get; set; }

    /// <summary>Item quantity.</summary>
    public required int Quantity { get; set; }

    /// <summary>Item price.</summary>
    public required decimal Price { get; set; }
}