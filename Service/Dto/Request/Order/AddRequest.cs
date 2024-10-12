using Entities.Models;

namespace Service.Dto.Request.Order;

public class AddRequest
{
    public UserResponse Requestor { get; set; }
    public long CartId { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string ShippingAddress { get; set; } = "Testing Address";
}