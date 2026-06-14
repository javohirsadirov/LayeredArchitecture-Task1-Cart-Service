// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

namespace LayeredArchitectureTask1CartService.Dtos.CartService;

/// <summary>
/// Represents a cart with its items.
/// </summary>
public class CartDto
{
    /// <summary>Gets or sets the cart unique key.</summary>
    public required string Key { get; set; }

    /// <summary>Gets or sets the list of items in the cart.</summary>
    public List<ItemDto> Items { get; set; } = [];
}
