// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

using LayeredArchitectureTask1CartService.Business.CartServices.Exceptions;
using LayeredArchitectureTask1CartService.Business.CartServices.Implementation;
using LayeredArchitectureTask1CartService.Dtos.CartService;
using LayeredArchitectureTask1CartService.Repository.CartService.Interfaces;
using LayeredArchitectureTask1CartService.Repository.Models;
using Moq;

namespace LayeredArchitectureTask1CartService.Tests;

/// <summary>
/// Unit tests for the CartService business logic.
/// </summary>
public class CartServiceTests
{
    private Mock<ICartRepository> cartRepositoryMock = null!;
    private CartService cartService = null!;

    /// <summary>
    /// Initializes mocks and the CartService instance before each test.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        this.cartRepositoryMock = new Mock<ICartRepository>();
        this.cartService = new CartService(this.cartRepositoryMock.Object);
    }

    /// <summary>
    /// Verifies that adding an item with a negative price throws a ValidationException.
    /// </summary>
    [Test]
    public void AddItemAsyncThrowsValidationExceptionWhenPriceIsNegative()
    {
        var item = new ItemDto { Id = 1, Name = "Item", Quantity = 1, Price = -5m };

        Assert.ThrowsAsync<ValidationException>(async () =>
            await this.cartService.AddItemAsync("cart-1", item));
    }

    /// <summary>
    /// Verifies that adding an item with zero quantity throws a ValidationException.
    /// </summary>
    [Test]
    public void AddItemAsyncThrowsValidationExceptionWhenQuantityIsZero()
    {
        var item = new ItemDto { Id = 1, Name = "Item", Quantity = 0, Price = 10m };

        Assert.ThrowsAsync<ValidationException>(async () =>
            await this.cartService.AddItemAsync("cart-1", item));
    }

    /// <summary>
    /// Verifies that adding an item with an empty name throws a ValidationException.
    /// </summary>
    [Test]
    public void AddItemAsyncThrowsValidationExceptionWhenNameIsEmpty()
    {
        var item = new ItemDto { Id = 1, Name = "  ", Quantity = 1, Price = 10m };

        Assert.ThrowsAsync<ValidationException>(async () =>
            await this.cartService.AddItemAsync("cart-1", item));
    }

    /// <summary>
    /// Verifies that adding a valid item calls the repository correctly.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous test.</returns>
    [Test]
    public async Task AddItemAsyncCallsRepositoryWhenValid()
    {
        var item = new ItemDto { Id = 1, Name = "Valid Item", Quantity = 2, Price = 10m };
        this.cartRepositoryMock
            .Setup(r => r.AddItemAsync("cart-1", It.IsAny<Item>()))
            .Returns(Task.CompletedTask);

        await this.cartService.AddItemAsync("cart-1", item);

        this.cartRepositoryMock.Verify(
            r => r.AddItemAsync(
                "cart-1",
                It.Is<Item>(i => i.Id == 1 && i.Name == "Valid Item" && i.Quantity == 2 && i.Price == 10m)),
            Times.Once);
    }

    /// <summary>
    /// Verifies that UpdateItemsByProductIdAsync calls the repository.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous test.</returns>
    [Test]
    public async Task UpdateItemsByProductIdAsyncCallsRepository()
    {
        this.cartRepositoryMock
            .Setup(r => r.UpdateItemsByProductIdAsync(1, "Updated", 20m, "http://img.png", "alt"))
            .Returns(Task.CompletedTask);

        await this.cartService.UpdateItemsByProductIdAsync(1, "Updated", 20m, "http://img.png", "alt");

        this.cartRepositoryMock.Verify(r => r.UpdateItemsByProductIdAsync(1, "Updated", 20m, "http://img.png", "alt"), Times.Once);
    }

    /// <summary>
    /// Verifies that GetCartAsync returns null when the cart does not exist.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous test.</returns>
    [Test]
    public async Task GetCartAsyncReturnsNullWhenCartDoesNotExist()
    {
        this.cartRepositoryMock
            .Setup(r => r.GetCartAsync("nonexistent"))
            .ReturnsAsync((Cart?)null);

        var result = await this.cartService.GetCartAsync("nonexistent");

        Assert.That(result, Is.Null);
    }

    /// <summary>
    /// Verifies that GetCartAsync returns a CartDto when the cart exists.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous test.</returns>
    [Test]
    public async Task GetCartAsyncReturnsCartDtoWhenCartExists()
    {
        var cart = new Cart
        {
            Key = "cart-1",
            Items = [new Item { Id = 1, CartKey = "cart-1", Name = "Item1", Quantity = 1, Price = 5m }],
        };
        this.cartRepositoryMock
            .Setup(r => r.GetCartAsync("cart-1"))
            .ReturnsAsync(cart);

        var result = await this.cartService.GetCartAsync("cart-1");

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Key, Is.EqualTo("cart-1"));
        Assert.That(result.Items, Has.Count.EqualTo(1));
        Assert.That(result.Items[0].Name, Is.EqualTo("Item1"));
    }

    /// <summary>
    /// Verifies that RemoveItemAsync returns true when the repository returns true.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous test.</returns>
    [Test]
    public async Task RemoveItemAsyncReturnsTrueWhenRepositoryReturnsTrue()
    {
        this.cartRepositoryMock
            .Setup(r => r.RemoveItemAsync("cart-1", 1))
            .ReturnsAsync(true);

        var result = await this.cartService.RemoveItemAsync("cart-1", 1);

        Assert.That(result, Is.True);
    }

    /// <summary>
    /// Verifies that RemoveItemAsync returns false when the repository returns false.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous test.</returns>
    [Test]
    public async Task RemoveItemAsyncReturnsFalseWhenRepositoryReturnsFalse()
    {
        this.cartRepositoryMock
            .Setup(r => r.RemoveItemAsync("cart-1", 999))
            .ReturnsAsync(false);

        var result = await this.cartService.RemoveItemAsync("cart-1", 999);

        Assert.That(result, Is.False);
    }
}
