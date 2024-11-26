namespace Core.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Decimal Price { get; set; }
    public int Stock { get; set; }
    public string UserId { get; set; }
}