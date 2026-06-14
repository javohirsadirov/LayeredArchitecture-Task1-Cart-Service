// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

using System.ComponentModel.DataAnnotations;

namespace LayeredArchitectureTask1CartService.Dtos.CartService;

/// <summary>
/// Represents a cart item.
/// </summary>
public class ItemDto
{
    /// <summary>Gets or sets the item unique identifier.</summary>
    public required int Id { get; set; }

    /// <summary>Gets or sets the item name.</summary>
    public required string Name { get; set; } = string.Empty;

    /// <summary>Gets or sets the image URL.</summary>
    public string? ImageUrl { get; set; }

    /// <summary>Gets or sets the image alt text.</summary>
    public string? ImageAltText { get; set; }

    /// <summary>Gets or sets the item quantity.</summary>
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    public required int Quantity { get; set; }

    /// <summary>Gets or sets the item price.</summary>
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public required decimal Price { get; set; }
}
