// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

namespace LayeredArchitectureTask1CartService.MessageQueue;

/// <summary>
/// Configuration options for RabbitMQ connection.
/// </summary>
public class RabbitMQOptions
{
    /// <summary>The configuration section name.</summary>
    public const string SectionName = "RabbitMQOptions";

    /// <summary>Gets or sets the host name.</summary>
    public string HostName { get; set; } = "localhost";

    /// <summary>Gets or sets the port.</summary>
    public int Port { get; set; } = 5672;

    /// <summary>Gets or sets the product updated settings.</summary>
    public ProductUpdatedSettings ProductUpdated { get; set; } = new();
}
