using Entities.Models;
using Service.Dto.Response;

namespace Service.Dto.Request.Admin
{
    public class GetSellerRequestList
    {
        public UserResponse Requestor { get; set; }
        public SellerRequestResponse? SellerRequest { get; set; }
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SearchTerm { get; set; }
        public SellerReqeustStatus? Status { get; set; }
    }
}