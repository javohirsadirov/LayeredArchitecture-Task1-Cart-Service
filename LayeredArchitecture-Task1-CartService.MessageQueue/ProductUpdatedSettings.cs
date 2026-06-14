// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

namespace LayeredArchitectureTask1CartService.MessageQueue;

/// <summary>
/// Configuration settings for the product updated exchange and queue.
/// </summary>
public class ProductUpdatedSettings
{
    /// <summary>Gets or sets the exchange name.</summary>
    public string Exchange { get; set; } = string.Empty;

    /// <summary>Gets or sets the queue name.</summary>
    public string Queue { get; set; } = string.Empty;

    /// <summary>Gets or sets the routing key.</summary>
    public string RoutingKey { get; set; } = string.Empty;
}
