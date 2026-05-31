using LayeredArchitecture_Task1_CartService.MessageQueue.Implementation;
using Microsoft.Extensions.DependencyInjection;

namespace LayeredArchitecture_Task2_Catalog_Service.MessageQueue;

public static class MessageQueueServiceExtensions
{
    public static IServiceCollection AddMessageQueueConsumer(this IServiceCollection services)
    {
        services.AddHostedService<ProductUpdatedConsumerService>();
        return services;
    }
}