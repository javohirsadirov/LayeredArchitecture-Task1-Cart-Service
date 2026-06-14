// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

namespace LayeredArchitectureTask1CartService.Repository.Models;

/// <summary>
/// Represents a cart item stored in the repository.
/// </summary>
public class Item
{
    /// <summary>Gets or sets the item identifier.</summary>
    public required int Id { get; set; }

    /// <summary>Gets or sets the cart key this item belongs to.</summary>
    public required string CartKey { get; set; }

    /// <summary>Gets or sets the item name.</summary>
    public required string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the image URL.</summary>
    public string? ImageUrl { get; set; }

    /// <summary>Gets or sets the image alt text.</summary>
    public string? ImageAltText { get; set; }

    /// <summary>Gets or sets the quantity.</summary>
    public required int Quantity { get; set; }

    /// <summary>Gets or sets the price.</summary>
    public required decimal Price { get; set; }
}
