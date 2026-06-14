// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

using LayeredArchitectureTask1CartService.Repository.CartService.Implementation;
using LayeredArchitectureTask1CartService.Repository.CartService.Interfaces;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;

namespace LayeredArchitectureTask1CartService.Repository;

/// <summary>
/// Extension methods for registering repository services.
/// </summary>
public static class RegisterRepositoryServices
{
    /// <summary>
    /// Adds the repository services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
    {
        services.AddTransient<ICartRepository, CartRepository>();

        services.AddSingleton<LiteDatabase>(_ =>
            new LiteDatabase("Filename=mydata.db;Connection=Shared;"));

        return services;
    }
}
