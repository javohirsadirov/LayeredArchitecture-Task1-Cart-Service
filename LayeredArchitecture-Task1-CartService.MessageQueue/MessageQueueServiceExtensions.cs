using LayeredArchitecture_Task1_CartService.MessageQueue.Implementation;
using LayeredArchitecture_Task1_CartService.MessageQueue.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LayeredArchitecture_Task2_Catalog_Service.MessageQueue;

public static class MessageQueueServiceExtensions
{
    public static IServiceCollection AddMessageQueueConsumer(this IServiceCollection services)
    {
        services.AddSingleton<IMessageConsumer>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<RabbitMQOptions>>();
            var settings = options.Value;
            return new RabbitMQConsumer(settings.HostName, settings.Port);
        });

        services.AddHostedService<ProductUpdatedConsumerService>();

        return services;
    }
}