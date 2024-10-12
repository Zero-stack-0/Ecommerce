namespace Entities.Models;
public class Order
{
    public Order()
    { }
    public Order(long orderById, long productId, string productName, string productDescription, decimal priceAtOrderTime, decimal discountAtOrderTime, int quantity, OrderStatus status, string shippingAddress, PaymentMethod paymentMethod)
    {
        OrderById = orderById;
        ProductId = productId;
        ProductName = productName;
        ProductDescription = productDescription;
        DiscountAtOrderTime = discountAtOrderTime;
        PriceAtOrderTime = priceAtOrderTime;
        Quantity = quantity;
        ShippingAddress = shippingAddress;
        OrderStatus = status;
        TotalAmount = CalculatedAmount();
        TotalPriceWithoutDiscount = CalculatedTotalPriceWithoutDiscount();
        TotalDiscountPrice = CalculatedTotalDiscountPrice();
        PaymentMethod = paymentMethod;
        CreatedAt = DateTime.UtcNow;
    }
    public long Id { get; set; }
    public long OrderById { get; set; }
    public long ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductDescription { get; set; }
    public decimal PriceAtOrderTime { get; set; }
    public decimal DiscountAtOrderTime { get; set; }
    public int Quantity { get; set; }
    public decimal TotalAmount { get; set; }
    public string PaymentIntentId { get; set; }
    public decimal TotalPriceWithoutDiscount { get; set; }
    public decimal TotalDiscountPrice { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string ShippingAddress { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string TransactionId { get; set; }
    public Users OrderBy { get; set; }
    public Product Product { get; set; }

    public decimal CalculatedAmount()
    {
        return Quantity * (PriceAtOrderTime - (PriceAtOrderTime * DiscountAtOrderTime / 100));
    }

    public decimal CalculatedTotalPriceWithoutDiscount()
    {
        return Quantity * PriceAtOrderTime;
    }

    public decimal CalculatedTotalDiscountPrice()
    {
        return Quantity * (PriceAtOrderTime * DiscountAtOrderTime / 100);
    }
}

public enum OrderStatus
{
    Pending = 1,
    Completed,
    Shipped,
    Delivered,
    Cancelled
}



