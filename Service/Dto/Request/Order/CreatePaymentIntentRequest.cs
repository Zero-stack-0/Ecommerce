namespace Service.Dto.Request.Order;
public class CreatePaymentIntentRequest
{
    public UserResponse Requestor { get; set; }
    public long CartId { get; set; }
}