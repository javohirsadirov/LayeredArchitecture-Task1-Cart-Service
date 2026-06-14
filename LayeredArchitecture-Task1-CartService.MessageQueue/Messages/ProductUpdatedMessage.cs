// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

namespace LayeredArchitectureTask1CartService.MessageQueue.Messages;

/// <summary>
/// Represents a product updated message from the message queue.
/// </summary>
public class ProductUpdatedMessage
{
    /// <summary>Gets or sets the product identifier.</summary>
    public required int Id { get; set; }

    /// <summary>Gets or sets the product name.</summary>
    public required string Name { get; set; }

    /// <summary>Gets or sets the product price.</summary>
    public required decimal Price { get; set; }

    /// <summary>Gets or sets the image URL.</summary>
    public string? ImageUrl { get; set; }

    /// <summary>Gets or sets the image alt text.</summary>
    public string? ImageAltText { get; set; }
}
