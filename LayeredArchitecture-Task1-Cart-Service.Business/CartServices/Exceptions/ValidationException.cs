namespace LayeredArchitecture_Task1_Cart_Service.Business.CartServices.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}
