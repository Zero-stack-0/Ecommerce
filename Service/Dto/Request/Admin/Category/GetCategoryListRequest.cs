namespace Service.Dto.Request.Admin.Category
{
    public class GetCategoryListRequest
    {
        public UserResponse Requestor { get; set; }
        public Entities.Models.Category? Category { get; set; }
        public int PageNo { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string SearchTerm { get; set; }
    }
}