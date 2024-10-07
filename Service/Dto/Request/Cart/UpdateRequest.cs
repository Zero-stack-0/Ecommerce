namespace Service.Dto.Request.Cart
{
    public class UpdateRequest
    {
        public UserResponse Requestor { get; set; }
        public int Quantity { get; set; }
        public long CartId { get; set; }
    }
}