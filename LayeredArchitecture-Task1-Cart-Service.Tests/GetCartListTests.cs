// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

using LayeredArchitectureTask1CartService.Business.CartServices.Interfaces;
using LayeredArchitectureTask1CartService.Controllers.V1;
using LayeredArchitectureTask1CartService.Dtos.CartService;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace LayeredArchitectureTask1CartService.Tests;

/// <summary>
/// Unit tests for the get-cart controller action.
/// </summary>
public class GetCartListTests
{
    private Mock<ICartService> cartServiceMock = null!;
    private CartController controller = null!;

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
    /// Verifies that GetCart returns an OkObjectResult with the cart.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous test.</returns>
    [Test]
    public async Task GetCartReturnsOkResultWithCart()
    {
        var key = "cart-1";
        var expectedCart = new CartDto
        {
            Key = key,
            Items = [new ItemDto { Id = 1, Name = "Item1", Quantity = 1, Price = 9.99m }],
        };
        this.cartServiceMock
            .Setup(s => s.GetCartAsync(key))
            .ReturnsAsync(expectedCart);

        var result = await this.controller.GetCart(key);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = (OkObjectResult)result;
        Assert.That(okResult.Value, Is.EqualTo(expectedCart));
    }

    /// <summary>
    /// Verifies that GetCart returns NotFound when the cart does not exist.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous test.</returns>
    [Test]
    public async Task GetCartReturnsNotFoundWhenCartDoesNotExist()
    {
        var key = "nonexistent";
        this.cartServiceMock
            .Setup(s => s.GetCartAsync(key))
            .ReturnsAsync((CartDto?)null);

        var result = await this.controller.GetCart(key);

        Assert.That(result, Is.InstanceOf<NotFoundResult>());
    }

    /// <summary>
    /// Verifies that GetCart calls the service with the correct key.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous test.</returns>
    [Test]
    public async Task GetCartCallsServiceWithCorrectKey()
    {
        var key = "cart-1";
        this.cartServiceMock
            .Setup(s => s.GetCartAsync(key))
            .ReturnsAsync(new CartDto { Key = key });

        await this.controller.GetCart(key);

        this.cartServiceMock.Verify(s => s.GetCartAsync(key), Times.Once);
    }
}
