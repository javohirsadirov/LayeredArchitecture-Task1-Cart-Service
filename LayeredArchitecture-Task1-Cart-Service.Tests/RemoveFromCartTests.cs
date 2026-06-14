// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

using LayeredArchitectureTask1CartService.Business.CartServices.Interfaces;
using LayeredArchitectureTask1CartService.Controllers.V1;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LayeredArchitectureTask1CartService.Tests;

/// <summary>
/// Unit tests for the delete-item controller action.
/// </summary>
public class RemoveFromCartTests
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
    /// Verifies that DeleteItem returns Ok when the item exists.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous test.</returns>
    [Test]
    public async Task DeleteItemReturnsOkWhenItemExists()
    {
        var key = "cart-1";
        var itemId = 1;
        this.cartServiceMock
            .Setup(s => s.RemoveItemAsync(key, itemId))
            .ReturnsAsync(true);

        var result = await this.controller.DeleteItem(key, itemId);

        Assert.That(result, Is.InstanceOf<OkResult>());
    }

    /// <summary>
    /// Verifies that DeleteItem returns NotFound when the item does not exist.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous test.</returns>
    [Test]
    public async Task DeleteItemReturnsNotFoundWhenItemDoesNotExist()
    {
        var key = "cart-1";
        var itemId = 999;
        this.cartServiceMock
            .Setup(s => s.RemoveItemAsync(key, itemId))
            .ReturnsAsync(false);

        var result = await this.controller.DeleteItem(key, itemId);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    /// <summary>
    /// Verifies that DeleteItem calls the service with the correct parameters.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous test.</returns>
    [Test]
    public async Task DeleteItemCallsServiceWithCorrectParameters()
    {
        var key = "cart-1";
        var itemId = 1;
        this.cartServiceMock
            .Setup(s => s.RemoveItemAsync(key, itemId))
            .ReturnsAsync(true);

        await this.controller.DeleteItem(key, itemId);

        this.cartServiceMock.Verify(s => s.RemoveItemAsync(key, itemId), Times.Once);
    }
}
