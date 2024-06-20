namespace Service.Dto.Request.Admin
{
    public class GetUserListRequest
    {
        public UserResponse? Requestor { get; set; }
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SearchTerm { get; set; }
    }
}