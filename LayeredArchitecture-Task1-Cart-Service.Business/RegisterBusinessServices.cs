using Microsoft.Extensions.DependencyInjection;
using LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Interfaces;
using LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Implementation;

namespace LayeredArchitecture_Task1_Cart_Service.Business;

public static class RegisterBusinessServices
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddTransient<ICartService, CartService>();

        return services;
    }
}