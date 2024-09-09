namespace Service.Dto.Request.Admin.Category
{
    public class UpdateRequest
    {
        public UserResponse Requestor { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}