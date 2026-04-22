using LayeredArchitecture_Task1_Cart_Service.Repository.CartService.Implementation;
using LayeredArchitecture_Task1_Cart_Service.Repository.CartService.Interfaces;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;

namespace LayeredArchitecture_Task1_Cart_Service.Repository;

public static class RegisterRepositoryServices
{
    public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
    {
        services.AddTransient<ICartRepository, CartRepository>();

        services.AddSingleton<LiteDatabase>(_ =>
            new LiteDatabase("Filename=mydata.db;Connection=Shared;"));

        return services;
    }
}