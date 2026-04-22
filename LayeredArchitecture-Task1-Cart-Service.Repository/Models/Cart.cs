namespace LayeredArchitecture_Task1_Cart_Service.Repository.Models;

public class Cart
{
    public required Guid Id { get; set; }
    public List<Item> CartItems { get; set; } = [];
}