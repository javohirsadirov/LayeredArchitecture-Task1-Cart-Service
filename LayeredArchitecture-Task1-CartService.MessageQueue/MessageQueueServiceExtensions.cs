// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

using LayeredArchitectureTask1CartService.MessageQueue.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace LayeredArchitectureTask1CartService.MessageQueue;

/// <summary>
/// Extension methods for registering message queue services.
/// </summary>
public static class MessageQueueServiceExtensions
{
    /// <summary>
    /// Adds the message queue consumer as a hosted service.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMessageQueueConsumer(this IServiceCollection services)
    {
        services.AddHostedService<ProductUpdatedConsumerService>();
        return services;
    }
}
