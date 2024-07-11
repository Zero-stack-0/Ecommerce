using Entities.Models;

namespace Service.Dto.Request
{
    public class SellerRequestDto
    {
        public UserResponse Requestor { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string StoreContactNumber { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}