namespace Service.Dto.Request.Admin.Category
{
    public class AddCategoryRequest
    {
        public UserResponse Requestor { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}