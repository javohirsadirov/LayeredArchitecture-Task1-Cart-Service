// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

namespace LayeredArchitectureTask1CartService.Repository.Models;

/// <summary>
/// Represents a shopping cart.
/// </summary>
public class Cart
{
    /// <summary>Gets or sets the cart unique key.</summary>
    public required string Key { get; set; }

    /// <summary>Gets or sets the list of items in the cart.</summary>
    public List<Item> Items { get; set; } = [];
}
