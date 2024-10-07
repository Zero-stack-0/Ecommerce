namespace Entities.Models;

public class Cart
{
    public Cart()
    { }
    public Cart(long productId, long addedById, int quantity, decimal priceAtCartTime, decimal discountAtCartTime)
    {
        ProductId = productId;
        AddedById = addedById;
        Quantity = quantity;
        PriceAtCartTime = priceAtCartTime;
        DiscountAtCartTime = discountAtCartTime;
        CreatedAt = DateTime.UtcNow;
        IsPurchased = false;
        IsDeleted = false;
    }
    public long Id { get; set; }
    public long ProductId { get; set; }
    public long AddedById { get; set; }
    public decimal PriceAtCartTime { get; set; }
    public decimal DiscountAtCartTime { get; set; }
    public bool IsPurchased { get; set; }
    public int Quantity { get; set; }
    public Product Product { get; set; }
    public Users AddedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public void Update(int quantity)
    {
        Quantity = quantity;
        UpdatedAt = DateTime.UtcNow;
    }
}