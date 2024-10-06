namespace Service.Dto.Request.Cart
{
    public class AddRequest
    {
        public UserResponse Requestor { get; set; }
        public int Quantity { get; set; }
        public long ProductId { get; set; }
    }
}