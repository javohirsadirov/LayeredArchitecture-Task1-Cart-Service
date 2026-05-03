namespace LayeredArchitecture_Task1_Cart_Service.Repository.Models;

public class Cart
{
    public required string Key { get; set; }
    public List<Item> Items { get; set; } = [];
}