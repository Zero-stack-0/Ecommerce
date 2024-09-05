using Entities.Models;

namespace Service.Dto.Request.Admin
{
    public class UpdateSellerRequestStatus
    {
        public UserResponse Requestor { get; set; }
        public long SellerRequestId { get; set; }
        public SellerReqeustStatus Status { get; set; }
    }
}