using Entities;
using Entities.Models;

namespace Service.Dto.Response
{
    public class SellerRequestResponse
    {
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string StoreContactNumber { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public bool IsApproved { get; set; }
        public long UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public SellerReqeustStatus Status { get; set; }
    }
}