// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

using LayeredArchitectureTask1CartService.Business.CartServices.Exceptions;
using LayeredArchitectureTask1CartService.Business.CartServices.Interfaces;
using LayeredArchitectureTask1CartService.Controllers.V1;
using LayeredArchitectureTask1CartService.Dtos.CartService;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LayeredArchitectureTask1CartService.Tests;

/// <summary>
/// Unit tests for add-to-cart controller actions.
/// </summary>
public class AddToCartTests
{
    private Mock<ICartService> cartServiceMock;
    private CartController controller;

    /// <summary>
    /// Initializes mocks and the controller before each test.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        this.cartServiceMock = new Mock<ICartService>();
        this.controller = new CartController(this.cartServiceMock.Object);
    }

    /// <summary>
    /// Verifies that AddItem returns an OkResult on success.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Test]
    public async Task AddItemReturnsOkResult()
    {
        var key = "cart-1";
        var item = new ItemDto { Id = 1, Name = "Test Item", Quantity = 1, Price = 9.99m };
        this.cartServiceMock
            .Setup(s => s.AddItemAsync(key, item))
            .Returns(Task.CompletedTask);

        var result = await this.controller.AddItem(key, item);

        Assert.That(result, Is.InstanceOf<OkResult>());
    }

    /// <summary>
    /// Verifies that AddItem calls the service with the correct parameters.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous test.</returns>
    [Test]
    public async Task AddItemCallsServiceWithCorrectParameters()
    {
        var key = "cart-1";
        var item = new ItemDto { Id = 2, Name = "Test Item", Quantity = 2, Price = 19.99m };
        this.cartServiceMock
            .Setup(s => s.AddItemAsync(key, item))
            .Returns(Task.CompletedTask);

        await this.controller.AddItem(key, item);

        this.cartServiceMock.Verify(s => s.AddItemAsync(key, item), Times.Once);
    }

    /// <summary>
    /// Verifies that AddItem returns a BadRequest when validation fails.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous test.</returns>
    [Test]
    public async Task AddItemReturnsBadRequestWhenValidationFails()
    {
        var key = "cart-1";
        var item = new ItemDto { Id = 1, Name = "Test Item", Quantity = -1, Price = 5.00m };
        this.cartServiceMock
            .Setup(s => s.AddItemAsync(key, item))
            .ThrowsAsync(new ValidationException("Quantity must be greater than zero."));

        var result = await this.controller.AddItem(key, item);

        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
    }

    /// <summary>
    /// Verifies that AddItem throws when the service fails with an unexpected error.
    /// </summary>
    [Test]
    public void AddItemThrowsExceptionWhenServiceFails()
    {
        var key = "cart-1";
        var item = new ItemDto { Id = 1, Name = "Test Item", Quantity = 1, Price = 5.00m };
        this.cartServiceMock
            .Setup(s => s.AddItemAsync(key, item))
            .ThrowsAsync(new InvalidOperationException("Service error"));

        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await this.controller.AddItem(key, item));
    }
}
