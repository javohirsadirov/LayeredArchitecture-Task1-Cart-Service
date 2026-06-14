// Copyright (c) LayeredArchitecture-Task1-Cart-Service. All rights reserved.

namespace LayeredArchitectureTask1CartService.Business.CartServices.Exceptions;

/// <summary>
/// ValidationException is thrown when the input data for adding an item to the cart is invalid,
/// such as negative price, non-positive quantity, or missing name.
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ValidationException(string message)
        : base(message)
    {
    }
}
