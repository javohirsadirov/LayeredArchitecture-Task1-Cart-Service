// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

using LayeredArchitectureTask1CartService.Business.CartServices.Implementation;
using LayeredArchitectureTask1CartService.Business.CartServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LayeredArchitectureTask1CartService.Business;

/// <summary>
/// Extension methods for registering business services in the DI container.
/// </summary>
public static class RegisterBusinessServices
{
    /// <summary>
    /// Adds the business services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddTransient<ICartService, CartService>();

        return services;
    }
}
