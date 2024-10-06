namespace Service.Dto.Request.Cart
{
    public class GetListRequest
    {
        public UserResponse Requestor { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string SearchTerm { get; set; }
    }
}